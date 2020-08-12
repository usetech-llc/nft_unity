using System;
using NftUnity.Extensions;
using NftUnity.MethodGroups;
using Polkadot.Api;
using Polkadot.BinaryContracts.Events;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Extensions;
using Polkadot.Utils;

namespace NftUnity
{
    public class NftClient : INftClient
    {
        private IApplication? _application = null;
        private bool _connectWasCalled = false;
        private string? _eventsSubscription = null;

        public ICollectionManagement CollectionManagement { get; } = null!;
        public IItemManagement ItemManagement { get; } = null!;
        public NftClientSettings Settings { get; } = null!;

        private NftClient()
        {
        }

        public NftClient(NftClientSettings settings)
        {
            Settings = settings;
            
            var param = new JsonRpcParams();
            param.JsonrpcVersion = "2.0";

            var jsonRpc = new JsonRpc(new Wsclient(settings.Logger), settings.Logger, param);
            _application = new Application(settings.Logger, jsonRpc, settings.SerializerSettings);
            
            CollectionManagement = new CollectionManagement(this);
            ItemManagement = new ItemManagement(this);
        }

        public IApplication GetApplication()
        {
            if (_application == null)
            {
                throw new ObjectDisposedException(nameof(NftClient));
            }

            if (!_connectWasCalled)
            {
                _connectWasCalled = true;
                Connect();
            }

            return _application;
        }


        public void Connect()
        {
            _application!.Connect(Settings.WsEndpoint);
        }

        private event EventHandler<IEvent>? NewEvent;
        public event EventHandler<Exception>? OnError;

        event EventHandler<IEvent> INftClient.NewEvent
        {
            add
            {
                NewEvent += value;
                EnsureBlockSubscribed();
            }
            remove
            {
                NewEvent -= value;
                UnsubscribeBlockIfNeeded();
            }
        }

        private void UnsubscribeBlockIfNeeded()
        {
            if (_eventsSubscription == null)
            {
                return;
            }

            if (NewEvent == null)
            {
                GetApplication().UnsubscribeStorage(_eventsSubscription);
                _eventsSubscription = null;
            }
        }

        private void EnsureBlockSubscribed()
        {
            if (_eventsSubscription != null)
            {
                return;
            }

            this.MakeCallWithReconnect(app =>
            {
                var storageKey = app.GetKeys("System", "Events");
                _eventsSubscription = app.SubscribeStorage(storageKey, change =>
                {
                    if (change == null)
                    {
                        return;
                    }

                    try
                    {
                        var events = GetApplication().Serializer
                            .DeserializeAssertReadAll<EventList>(change.HexToByteArray()).Events;
                        foreach (var eventRecord in events)
                        {
                            NewEvent?.Invoke(this, eventRecord.Event);
                        }
                    }
                    catch (Exception ex)
                    {
                        Settings.Logger.Error($"Failed to deserialize events: {ex}");
                        OnError?.Invoke(this, ex);
                        throw;
                    }
                });
            }, Settings.MaxReconnectCount);
        }

        public void Dispose()
        {
            _application?.Dispose();
            _application = null;
        }
    }
}
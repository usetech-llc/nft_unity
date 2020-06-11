using System;
using System.Threading;
using NftUnity.MethodGroups;
using Polkadot.Api;
using Polkadot.Utils;

namespace NftUnity
{
    public class NFtClient : INftClient
    {
        private NftClientSettings Settings { get; } = null!;
        private IApplication? _application = null;
        private bool _connectWasCalled = false;
        private readonly ManualResetEventSlim _connectLock = new ManualResetEventSlim(true);

        public ICollectionManagement CollectionManagement { get; } = null!;

        private NFtClient()
        {
        }

        public NFtClient(NftClientSettings settings)
        {
            Settings = settings;
            
            var param = new JsonRpcParams();
            param.JsonrpcVersion = "2.0";

            var jsonRpc = new JsonRpc(new Wsclient(settings.Logger), settings.Logger, param);
            _application = new Application(settings.Logger, jsonRpc);
            
            CollectionManagement = new CollectionManagement(MakeCallWithReconnect, AddressUtils.GetPublicKeyFromAddr, GetApplication);
        }

        private IApplication GetApplication()
        {
            if (_application == null)
            {
                throw new ObjectDisposedException(nameof(NFtClient));
            }
            
            if (!_connectWasCalled)
            {
                Connect();
            }

            return _application;
        }

        private void MakeCallWithReconnect(Action<IApplication> call)
        {
            try
            {
                call(GetApplication());
            }
            catch (Exception ex)
            {
                if (!IsDisconnectedException(ex))
                {
                    throw;
                }
                
                _connectLock.Wait();
                ConnectThreadUnsafe();
                _connectLock.Set();
                call(GetApplication());
            }
        }

        private void Connect()
        {
            _connectLock.Wait();
            if (_connectWasCalled)
            {
                _connectLock.Set();
                return;
            }

            ConnectThreadUnsafe();
            _connectWasCalled = true;
            _connectLock.Set();
        }

        private void ConnectThreadUnsafe()
        {
            _application!.Connect(Settings.WsEndpoint);
        }

        private bool IsDisconnectedException(Exception ex)
        {
            return ex is ApplicationException applicationException &&
                   string.Equals("Not connected", applicationException.Message);
        }

        public void Dispose()
        {
            _application?.Dispose();
            _application = null;
        }
    }
}
using System;
using NftUnity.MethodGroups;
using Polkadot.Api;
using Polkadot.Utils;

namespace NftUnity
{
    public class NFtClient : INftClient
    {
        public NftClientSettings Settings { get; } = null!;
        private IApplication? _application = null;
        private bool _connectWasCalled = false;

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
            
            CollectionManagement = new CollectionManagement(this);
        }

        public IApplication GetApplication()
        {
            if (_application == null)
            {
                throw new ObjectDisposedException(nameof(NFtClient));
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

        public void Dispose()
        {
            _application?.Dispose();
            _application = null;
        }
    }
}
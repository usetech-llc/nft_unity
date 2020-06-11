using System;
using System.Linq;
using NftUnity.Extensions;
using NftUnity.Models;
using Polkadot.Api;
using Polkadot.DataStructs;

namespace NftUnity.MethodGroups
{
    internal class CollectionManagement : ICollectionManagement
    {
        private const string Module = "Nft";
        private const string CollectionStorage = "Collection";
        private const string CreateCollectionMethod = "create_collection";
        
        private readonly Action<Action<IApplication>> _apiCaller;
        private readonly Func<Address, PublicKey> _publicKeyParser;
        private readonly Func<IApplication> _applicationGetter;
        private int? _collectionCreatedSubscriptionId = null;

        internal CollectionManagement(Action<Action<IApplication>> apiCaller, Func<Address, PublicKey> publicKeyParser, Func<IApplication> applicationGetter)
        {
            _apiCaller = apiCaller;
            _publicKeyParser = publicKeyParser;
            _applicationGetter = applicationGetter;
        }
        
        public void CreateCollection(uint customDataSize, Address sender, string privateKey)
        {
            _apiCaller(application =>
            {
                var key = _publicKeyParser(sender);
                var param = customDataSize.ToCompactBytes();

                var methodBytes = key.Bytes.Concat(param).ToArray();
                application.SubmitExtrinsic(
                    methodBytes, 
                    Module, 
                    CreateCollectionMethod, 
                    sender,
                    privateKey);
            });
        }

        private event EventHandler<CollectionCreatedEventArgs>? CollectionCreated;

        event EventHandler<CollectionCreatedEventArgs>? ICollectionManagement.CollectionCreated
        {
            add
            {
                if (_collectionCreatedSubscriptionId == null)
                {
                    var app = _applicationGetter();
                    _collectionCreatedSubscriptionId = app.SubscribeStorage(CollectionCreatedKey(app), OnCollectionCreated);
                }
                this.CollectionCreated += value;
                
            }
            remove
            {
                this.CollectionCreated -= value;
                var id = _collectionCreatedSubscriptionId;
                if (CollectionCreated == null && id != null)
                {
                    var app = _applicationGetter();
                    app.UnsubscribeStorage(id.Value);
                }
            }
        }

        private void OnCollectionCreated(string obj)
        {
            CollectionCreated?.Invoke(this, new CollectionCreatedEventArgs("", ""));
        }

        private static string CollectionCreatedKey(IApplication app)
        {
            return app.GetKeys("", Module, CollectionStorage);
        }
    }
}
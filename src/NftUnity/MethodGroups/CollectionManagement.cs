using System;
using System.Linq;
using System.Threading.Tasks;
using NftUnity.Extensions;
using NftUnity.Models;
using NftUnity.Models.Events;
using Polkadot.Api;
using Polkadot.BinaryContracts.Events;
using Polkadot.DataStructs;
using Polkadot.Utils;

namespace NftUnity.MethodGroups
{
    internal class CollectionManagement : ICollectionManagement
    {
        private const string MODULE = "Nft";
        private const string CREATE_COLLECTION_METHOD = "create_collection";
        private const string COLLECTION_STORAGE = "Collection";
        private const string DESTROY_COLLECTION_METHOD = "destroy_collection";
        private const string CHANGE_OWNER_METHOD = "change_collection_owner";

        private bool _eventSubscribed = false;
        private readonly INftClient _nftClient;

        internal CollectionManagement(INftClient nftClient)
        {
            _nftClient = nftClient;
        }
        
        public string CreateCollection(CreateCollection createCollection, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application => application.SubmitExtrinsicObject(
                createCollection, 
                MODULE, 
                CREATE_COLLECTION_METHOD, 
                sender,
                privateKey));
        }

        public string ChangeCollectionOwner(ChangeOwner changeOwner, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application => application.SubmitExtrinsicObject(
                changeOwner, 
                MODULE, 
                CHANGE_OWNER_METHOD, 
                sender,
                privateKey));
        }

        public void DestroyCollection(uint collectionId, Address sender, string privateKey)
        {
        }

        private event EventHandler<Created> CollectionCreated;
        public Collection GetCollection(ulong id)
        {
            var application = _nftClient.GetApplication();
            var request = application.GetStorage(id, MODULE, COLLECTION_STORAGE);
            return application.Serializer.Deserialize<Collection>(request.HexToByteArray());
        }

        event EventHandler<Created> ICollectionManagement.CollectionCreated
        {
            add
            {
                if (!_eventSubscribed)
                {
                    _eventSubscribed = true;
                    _nftClient.NewEvent += OnNewEvent;
                }
                this.CollectionCreated += value;
            }
            remove
            {
                this.CollectionCreated -= value;
                if (CollectionCreated == null)
                {
                    _eventSubscribed = false;
                    _nftClient.NewEvent -= OnNewEvent;
                }
            }
        }

        private void OnNewEvent(object sender, IEvent e)
        {
            if (e is Created created)
            {
                CollectionCreated?.Invoke(sender, created);
            }
        }
    }
}
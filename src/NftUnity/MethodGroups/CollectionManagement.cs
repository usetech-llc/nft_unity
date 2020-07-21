using System;
using System.Linq;
using System.Threading.Tasks;
using NftUnity.Extensions;
using NftUnity.Models;
using Polkadot.Api;
using Polkadot.DataStructs;
using Polkadot.Utils;

namespace NftUnity.MethodGroups
{
    internal class CollectionManagement : ICollectionManagement
    {
        private const string MODULE = "Nft";
        private const string CREATE_COLLECTION_METHOD = "create_collection";
        private const string DESTROY_COLLECTION_METHOD = "destroy_collection";
        
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

        public void ChangeCollectionOwner(uint collectionId, Address newOwner, Address sender, string privateKey)
        {
        }

        public void DestroyCollection(uint collectionId, Address sender, string privateKey)
        {
        }
    }
}
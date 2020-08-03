using System;
using NftUnity.Extensions;
using NftUnity.Models;
using NftUnity.Models.Collection;
using NftUnity.Models.Events;
using Polkadot.BinaryContracts;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Extensions;
using Polkadot.DataStructs;
using Polkadot.Utils;

namespace NftUnity.MethodGroups
{
    internal class CollectionManagement : ICollectionManagement
    {
        private const string Module = "Nft";
        
        private const string CreateCollectionMethod = "create_collection";
        private const string DestroyCollectionMethod = "destroy_collection";
        private const string ChangeOwnerMethod = "change_collection_owner";
        private const string AddAdminMethod = "add_collection_admin";
        private const string RemoveAdminMethod = "remove_collection_admin";
        private const string SetCollectionSponsorMethod = "set_collection_sponsor";
        private const string ConfirmSponsorshipMethod = "confirm_sponsorship";
        private const string RemoveSponsorshipMethod = "remove_collection_sponsor";

        private const string CollectionStorage = "Collection";
        private const string AdminListStorage = "AdminList";
        private const string BalanceStorage = "Balance";
        private const string NextIdStorage = "NextCollectionID";
        private const string AddressTokensStorage = "AddressTokens";

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
                Module, 
                CreateCollectionMethod, 
                sender,
                privateKey));
        }

        public string ChangeCollectionOwner(ChangeOwner changeOwner, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application => application.SubmitExtrinsicObject(
                changeOwner, 
                Module, 
                ChangeOwnerMethod, 
                sender,
                privateKey));
        }

        public string DestroyCollection(DestroyCollection destroyCollection, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application => application.SubmitExtrinsicObject(
                destroyCollection, 
                Module, 
                DestroyCollectionMethod, 
                sender,
                privateKey));
        }

        public string AddCollectionAdmin(AddCollectionAdmin addCollectionAdmin, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application => application.SubmitExtrinsicObject(
                addCollectionAdmin, 
                Module, 
                AddAdminMethod, 
                sender,
                privateKey));
        }

        public string RemoveCollectionAdmin(RemoveCollectionAdmin removeCollectionAdmin, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application => application.SubmitExtrinsicObject(
                removeCollectionAdmin, 
                Module, 
                RemoveAdminMethod, 
                sender,
                privateKey));
        }

        public string SetCollectionSponsor(SetCollectionSponsor setCollectionSponsor, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application =>
                application.SubmitExtrinsicObject(setCollectionSponsor, Module, SetCollectionSponsorMethod, sender, privateKey));
        }

        public string ConfirmSponsorship(ulong collectionId, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application =>
                application.SubmitExtrinsicObject(collectionId, Module, ConfirmSponsorshipMethod, sender, privateKey));
        }

        public string RemoveSponsor(RemoveCollectionSponsor removeCollectionSponsor, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application =>
                application.SubmitExtrinsicObject(removeCollectionSponsor, Module, RemoveSponsorshipMethod, sender, privateKey));
        }

        public ulong? BalanceOf(GetBalanceOf getBalanceOf)
        {
            return _nftClient.MakeCallWithReconnect(application => application.GetStorageObject<ulong?, DoubleMapKey<ulong, byte[]>>(new DoubleMapKey<ulong, byte[]>(getBalanceOf.CollectionId, AddressUtils.GetPublicKeyFromAddr(getBalanceOf.Account).Bytes), Module, BalanceStorage));
        }

        public AdminList? GetAdminList(ulong collectionId)
        {
            return _nftClient.MakeCallWithReconnect(application => application.GetStorageObject<AdminList, ulong>(collectionId, Module, AdminListStorage));
        }

        public TokensList? AddressTokens(AddressTokens addressTokens)
        {
            var publicKey = AddressUtils.GetPublicKeyFromAddr(addressTokens.Owner).Bytes;
            return _nftClient.MakeCallWithReconnect(application =>
                application.GetStorageObject<TokensList, DoubleMapKey<ulong, byte[]>>(
                    DoubleMapKey.Create(addressTokens.CollectionId, publicKey), Module,
                    AddressTokensStorage));
        }

        public ulong? NextCollectionId()
        {
            return _nftClient.MakeCallWithReconnect(application => application.GetStorageObject<ulong?, Empty>(Empty.Instance, Module, NextIdStorage));
        }

        private event EventHandler<Created>? CollectionCreated;
        public Collection? GetCollection(ulong id)
        {
            return _nftClient.MakeCallWithReconnect(application => application.GetStorageObject<Collection, ulong>(id, Module, CollectionStorage));
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
using System;
using NftUnity.Extensions;
using NftUnity.Models.Events;
using NftUnity.Models.Item;
using Polkadot.BinaryContracts;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.MethodGroups
{
    public class ItemManagement : IItemManagement
    {
        private const string Module = "Nft";
        
        private const string CreateItemMethod = "create_item";
        private const string BurnItemMethod = "burn_item";
        private const string TransferMethod = "transfer";
        private const string TransferFromMethod = "transfer_from";
        private const string ApproveMethod = "approve";
        
        private const string NftItemStorage = "NftItemList";
        private const string FungibleItemStorage = "FungibleItemType";
        private const string ReFungibleItemStorage = "ReFungibleItemList";
        private const string ApprovedStorage = "ApprovedList";
        private const string ItemListIndexStorage = "ItemListIndex";

        private readonly INftClient _nftClient;
        private bool _eventsSubscribed = false;

        public ItemManagement(INftClient nftClient)
        {
            _nftClient = nftClient;
        }

        public string CreateItem(CreateItem createItem, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application =>
                application.SubmitExtrinsicObject(createItem, Module, CreateItemMethod, sender, privateKey), _nftClient.Settings.MaxReconnectCount);
        }

        public string BurnItem(ItemKey key, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application =>
                application.SubmitExtrinsicObject(key, Module, BurnItemMethod, sender, privateKey), _nftClient.Settings.MaxReconnectCount);
        }

        public string Transfer(Transfer transfer, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application =>
                application.SubmitExtrinsicObject(transfer, Module, TransferMethod, sender, privateKey), _nftClient.Settings.MaxReconnectCount);
        }

        public string TransferFrom(TransferFrom transferFrom, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application =>
                application.SubmitExtrinsicObject(transferFrom, Module, TransferFromMethod, sender, privateKey), _nftClient.Settings.MaxReconnectCount);
        }

        public string Approve(Approve approve, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application =>
                application.SubmitExtrinsicObject(approve, Module, ApproveMethod, sender, privateKey), _nftClient.Settings.MaxReconnectCount);
        }

        public NftItem? GetNftItem(ItemKey key)
        {
            return _nftClient.MakeCallWithReconnect(application => application.GetStorageObject<NftItem, DoubleMapKey<ulong, ulong>>(StorageKey(key), Module, NftItemStorage), _nftClient.Settings.MaxReconnectCount);
        }

        private static DoubleMapKey<ulong, ulong> StorageKey(ItemKey key)
        {
            return DoubleMapKey.Create(key.CollectionId, key.ItemId);
        }

        public ApprovedList? GetApproved(ItemKey key)
        {
            return _nftClient.MakeCallWithReconnect(application => 
                application.GetStorageObject<ApprovedList, DoubleMapKey<ulong, ulong>>(StorageKey(key), Module, ApprovedStorage), _nftClient.Settings.MaxReconnectCount);
        }

        public PublicKey? GetOwner(ItemKey key)
        {
            return GetNftItem(key)?.Owner;
        }

        public ulong? NextId(ulong collectionId)
        {
            return _nftClient.MakeCallWithReconnect(application =>
                application.GetStorageObject<ulong?, MapKey<ulong>>(MapKey.Create(collectionId), Module, ItemListIndexStorage), _nftClient.Settings.MaxReconnectCount);
        }

        private event EventHandler<ItemCreated>? ItemCreated = null;
        private event EventHandler<ItemDestroyed>? ItemDestroyed = null;

        event EventHandler<ItemDestroyed> IItemManagement.ItemDestroyed
        {
            add
            {
                EnsureEventsSubscribed();
                this.ItemDestroyed += value;
            }
            remove
            {
                this.ItemDestroyed -= value;
                UnsubscribeEventsIfNeeded();
            }
        }

        event EventHandler<ItemCreated> IItemManagement.ItemCreated
        {
            add
            {
                EnsureEventsSubscribed();
                this.ItemCreated += value;
            }
            remove
            {
                this.ItemCreated -= value;
                UnsubscribeEventsIfNeeded();
            }
        }

        private void UnsubscribeEventsIfNeeded()
        {
            if (!_eventsSubscribed)
            {
                return;
            }

            if (this.ItemCreated == null && this.ItemDestroyed == null)
            {
                _eventsSubscribed = false;
                _nftClient.NewEvent -= NewEvent;
            }
        }

        private void EnsureEventsSubscribed()
        {
            if (_eventsSubscribed)
            {
                return;
            }

            _eventsSubscribed = true;
            _nftClient.NewEvent += NewEvent;
        }

        private void NewEvent(object sender, IEvent e)
        {
            switch (e)
            {
                case ItemCreated itemCreated:
                    ItemCreated?.Invoke(sender, itemCreated);
                    break;
                case ItemDestroyed itemDestroyed:
                    ItemDestroyed?.Invoke(sender, itemDestroyed);
                    break;
            }
        }
    }
}
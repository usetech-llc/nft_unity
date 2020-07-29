using System;
using NftUnity.Extensions;
using NftUnity.Models;
using NftUnity.Models.Calls.Item;
using NftUnity.Models.Events;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Extensions;
using Polkadot.DataStructs;
using Polkadot.Utils;

namespace NftUnity.MethodGroups
{
    public class ItemManagement : IItemManagement
    {
        private const string Module = "Nft";
        private const string CreateItemMethod = "create_item";
        private const string BurnItemMethod = "burn_item";
        private const string ItemStorage = "ItemList";

        private readonly INftClient _nftClient;
        private bool _eventsSubscribed = false;

        public ItemManagement(INftClient nftClient)
        {
            _nftClient = nftClient;
        }

        public string CreateItem(CreateItem createItem, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application =>
                application.SubmitExtrinsicObject(createItem, Module, CreateItemMethod, sender, privateKey));
        }

        public string BurnItem(ItemKey key, Address sender, string privateKey)
        {
            return _nftClient.MakeCallWithReconnect(application =>
                application.SubmitExtrinsicObject(key, Module, BurnItemMethod, sender, privateKey));
        }

        public Item? GetItem(ItemKey key)
        {
            return _nftClient.MakeCallWithReconnect(application =>
            {
                var request = application.GetStorage(DoubleMapKey.Create(key), Module, ItemStorage);
                if (string.IsNullOrEmpty(request))
                {
                    return null;
                }

                return application.Serializer.DeserializeAssertReadAll<Item>(request.HexToByteArray());
            });
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
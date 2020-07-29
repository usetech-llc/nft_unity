﻿using System;
using NftUnity.Extensions;
using NftUnity.Models;
using NftUnity.Models.Calls.Collection;
using NftUnity.Models.Events;
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
        private const string CollectionStorage = "Collection";
        private const string DestroyCollectionMethod = "destroy_collection";
        private const string ChangeOwnerMethod = "change_collection_owner";

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

        private event EventHandler<Created>? CollectionCreated;
        public Collection? GetCollection(ulong id)
        {
            return _nftClient.MakeCallWithReconnect(application =>
            {
                var request = application.GetStorage(id, Module, CollectionStorage);
                if (string.IsNullOrEmpty(request))
                {
                    return null;
                }

                return application.Serializer.DeserializeAssertReadAll<Collection>(request.HexToByteArray());
            });
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
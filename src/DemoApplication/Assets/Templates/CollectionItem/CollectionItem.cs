using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NftUnity;
using NftUnity.Models.Collection;
using NftUnity.Models.Events;
using NftUnity.Models.Item;
using Polkadot.Utils;
using UnityEngine;
using UnityEngine.UI;

public class CollectionItem : MonoBehaviour
{
    public InputField CollectionIdField;
    public InputField ModeField;
    public InputField OwnerField;
    public InputField AccessModeField;
    public InputField NameField;
    public InputField DescriptionField;
    public InputField TokenPrefixField;
    public InputField OffChainSchemaField;
    public InputField SponsorField;
    public InputField UnconfirmedField;

    public GameObject firstTokenItem;
    public GameObject tokenItemTemplate;
    public GameObject tokenContent;
    
    private List<(ItemKey key, NftItem item)> _tokens = new List<(ItemKey, NftItem)>();
    private List<GameObject> _tokenListItems = new List<GameObject>();
    private INftClient Client => NftClientFactory.CreateClient(GameSettings.Uri);
    private ulong? _collectionId;
    private ulong _selectedItemId;
    private ConcurrentQueue<Action> _updateQueue = new ConcurrentQueue<Action>();

    private void Start()
    {
        Client.ItemManagement.ItemCreated += ItemManagementOnItemCreated;
        Client.ItemManagement.ItemDestroyed -= ItemManagementOnItemDestroyed;
    }

    private void ItemManagementOnItemDestroyed(object sender, ItemDestroyed e)
    {
        if (e.Key.CollectionId != _collectionId)
        {
            return;
        }
        DestroyTokenItem(e.Key.ItemId);
        _tokens.RemoveAll(t => t.key == e.Key);
        _tokenListItems.RemoveAll(t => t.GetComponent<TokenItemScript>().Id == e.Key.ItemId);
        SortTokens();
    }

    private void OnDestroy()
    {
        Client.ItemManagement.ItemCreated -= ItemManagementOnItemCreated;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (_updateQueue.TryDequeue(out var update))
        {
            update();
        }
    }

    private void ItemManagementOnItemCreated(object sender, ItemCreated e)
    {
        if (e.Key.CollectionId != _collectionId)
        {
            return;
        }

        var nft = Client.ItemManagement.GetNftItem(e.Key);
        if (nft != null)
        {
            _updateQueue.Enqueue(() =>
            {
                AddToken(e.Key, nft);
                SortTokens();
            });
        }
    }

    private void AddToken(ItemKey key, NftItem nft)
    {
        _updateQueue.Enqueue(() =>
        {
            _tokens.Add((key, nft));

            var tokenListItem = Instantiate(tokenItemTemplate, firstTokenItem.transform);
            _tokenListItems.Add(tokenListItem);
            tokenListItem.transform.SetParent(firstTokenItem.transform.parent, false);
            var tokenScript = tokenListItem.GetComponent<TokenItemScript>();
            tokenScript.Id = key.ItemId;
            tokenScript.OnSelect += OnSelect;
        });
    }

    private void OnSelect(ulong itemId)
    {
        _selectedItemId = itemId;
        
        foreach (var item in _tokenListItems)
        {
            var script = item.GetComponent<TokenItemScript>();
            script.IsActive = itemId == script.Id;
        }

        if (_collectionId.HasValue)
        {
            var itemKey = new ItemKey(_collectionId.Value, itemId);
            var nft = Client.ItemManagement.GetNftItem(itemKey);
            if (nft != null && _selectedItemId == itemId)
            {
                var tokenContentScript = tokenContent.GetComponent<TokenContentScript>();
                tokenContentScript.SetToken(itemKey, nft);
            }
        }
    }

    private void SortTokens()
    {
        _updateQueue.Enqueue(() =>
        {
            var orderedTokenItems = _tokenListItems
                .Select(item => (item: item, script: item.GetComponent<TokenItemScript>()))
                .OrderByDescending(t => t.script.Id)
                .Select(t => t.item);

            var initialPosition = firstTokenItem.transform.position;
            var height = firstTokenItem.transform.GetComponent<RectTransform>().rect.height;
            var offset = 0f;
            foreach (var tokenListItem in orderedTokenItems)
            {
                var itemOffset = initialPosition.y - offset;
                var itemPosition = tokenListItem.transform.position;
                tokenListItem.transform.position = new Vector3(itemPosition.x, itemOffset, itemPosition.z);
                offset += height + 2;
            }
        
            firstTokenItem.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _tokenListItems.Count * (height + 2));
        });
    }

    private void DestroyTokenItem(ulong id)
    {
        foreach (var token in _tokenListItems.Where(t => t.GetComponent<TokenItemScript>().Id == id))
        {
            _updateQueue.Enqueue(() => Destroy(token));
        }
    }

    public void SetCollection(Collection collection, ulong collectionId)
    {
        _updateQueue.Enqueue(() =>
        {
            _collectionId = collectionId;
        
            CollectionIdField.text = collectionId.ToString();
            ModeField.text = collection.CollectionMode.Mode.Match(
                invalid => "Invalid",
                nft => $"Nft, data size {nft.CustomDataSize}",
                fungible => $"Fungible, amount {fungible.DecimalPoints}",
                reFungible => "Refungible");
            OwnerField.text = collection.Owner.ToAddress();
            AccessModeField.text = collection.AccessMode.Mode.Match(
                normal => "Normal",
                whiteList => "White List");
            NameField.text = collection.Name;
            DescriptionField.text = collection.Description;
            TokenPrefixField.text = collection.TokenPrefix;
            OffChainSchemaField.text = collection.OffChainSchema;
            SponsorField.text = collection.Sponsor.ToAddress();
            UnconfirmedField.text = collection.UnconfirmedSponsor.ToAddress();

            ClearTokens();
            for (ulong i = 1; i < 5000000; i++)
            {
                var key = new ItemKey(collectionId, i);
                var nft = Client.ItemManagement.GetNftItem(key);
                if (nft == null)
                {
                    break;
                }
            
                AddToken(key, nft);
            }

            if (_tokens.Any())
            {
                OnSelect(_tokens.Max(t => t.key.ItemId));
            }
            SortTokens();
        });
    }

    private void ClearTokens()
    {
        foreach (var token in _tokens)
        {
            DestroyTokenItem(token.key.ItemId);
        }
        
        _tokens.Clear();
        _tokenListItems.Clear();
    }
}

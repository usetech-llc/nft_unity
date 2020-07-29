using System;
using NftUnity.Models;
using NftUnity.Models.Calls.Item;
using NftUnity.Models.Events;
using Polkadot.DataStructs;

namespace NftUnity.MethodGroups
{
    /// <summary>
    /// Operations group to manage items.
    /// </summary>
    public interface IItemManagement
    {
        string CreateItem(CreateItem createItem, Address sender, string privateKey);
        string BurnItem(ItemKey key, Address sender, string privateKey);

        Item? GetItem(ItemKey key);

        event EventHandler<ItemCreated> ItemCreated;
        event EventHandler<ItemDestroyed> ItemDestroyed;
    }
}
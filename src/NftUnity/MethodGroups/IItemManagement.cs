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
        /// <summary>
        /// This method creates a concrete instance of NFT Collection created with CreateCollection method.
        /// </summary>
        /// <param name="createItem"></param>
        /// <param name="sender">Address, initial owner of the NFT.</param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        string CreateItem(CreateItem createItem, Address sender, string privateKey);
        
        /// <summary>
        /// This method destroys a concrete instance of NFT.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sender"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        string BurnItem(ItemKey key, Address sender, string privateKey);

        /// <summary>
        /// Change ownership of the token.
        /// </summary>
        /// <param name="transfer"></param>
        /// <param name="sender"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        string Transfer(Transfer transfer, Address sender, string privateKey);

        /// <summary>
        /// Set, change, or remove approved address to transfer the ownership of the NFT.
        /// </summary>
        /// <param name="approve"></param>
        /// <param name="sender"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        string Approve(Approve approve, Address sender, string privateKey);

        Item? GetItem(ItemKey key);

        /// <summary>
        /// Get the approved addresses for a single NFT.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ApprovedList? GetApproved(ItemKey key);

        event EventHandler<ItemCreated> ItemCreated;
        event EventHandler<ItemDestroyed> ItemDestroyed;
    }
}
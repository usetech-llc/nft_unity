using System;
using NftUnity.Models;
using NftUnity.Models.Events;
using NftUnity.Models.Item;
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
        /// Change ownership of a NFT on behalf of the owner. See Approve method for additional information. After this method executes, the approval is removed so that the approved address will not be able to transfer this NFT again from this owner.
        /// </summary>
        /// <param name="transferFrom"></param>
        /// <param name="sender"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        string TransferFrom(TransferFrom transferFrom, Address sender, string privateKey);

        /// <summary>
        /// Set, change, or remove approved address to transfer the ownership of the NFT.
        /// </summary>
        /// <param name="approve"></param>
        /// <param name="sender"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        string Approve(Approve approve, Address sender, string privateKey);

        NftItem? GetNftItem(ItemKey key);

        /// <summary>
        /// Get the approved addresses for a single NFT.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ApprovedList? GetApproved(ItemKey key);

        /// <summary>
        /// Non-Fungible Mode: Return the address of the NFT owner. 
        /// Fungible and Re-Fungible Mode: Not supported, returns the default address.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        PublicKey? GetOwner(ItemKey key);

        event EventHandler<ItemCreated> ItemCreated;
        event EventHandler<ItemDestroyed> ItemDestroyed;
    }
}
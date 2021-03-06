﻿using System;
using NftUnity.Models.Collection;
using NftUnity.Models.Events;
using Polkadot.DataStructs;

namespace NftUnity.MethodGroups
{
    /// <summary>
    /// Operations group to manage collections.
    /// </summary>
    public interface ICollectionManagement
    {
        /// <summary>
        /// This method creates a Collection of NFTs. Each Token may have multiple properties encoded as an array of bytes of certain length. The initial owner and admin of the collection are set to the address that signed the transaction. Both addresses can be changed later.
        /// </summary>
        /// <param name="createCollection"></param>
        /// <param name="sender">Address of initial owner and admin.</param>
        /// <param name="privateKey">Sender's private key.</param>
        string CreateCollection(CreateCollection createCollection, Address sender, string privateKey);

        /// <summary>
        /// Change the owner of the collection.
        /// </summary>
        /// <param name="changeOwner"></param>
        /// <param name="sender"></param>
        /// <param name="privateKey"></param>
        string ChangeCollectionOwner(ChangeOwner changeOwner, Address sender, string privateKey);

        /// <summary>
        /// DANGEROUS: Destroys collection and all NFTs within this collection. Users irrecoverably lose their assets and may lose real money.
        /// </summary>
        /// <param name="destroyCollection"></param>
        /// <param name="sender"></param>
        /// <param name="privateKey"></param>
        string DestroyCollection(DestroyCollection destroyCollection, Address sender, string privateKey);

        /// <summary>
        /// Gets collection by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Collection? GetCollection(ulong id);

        /// <summary>
        /// NFT Collection can be controlled by multiple admin addresses (some which can also be servers, for example). Admins can issue and burn NFTs, as well as add and remove other admins, but cannot change NFT or Collection ownership.
        /// </summary>
        /// <param name="addCollectionAdmin"></param>
        /// <returns></returns>
        string AddCollectionAdmin(AddCollectionAdmin addCollectionAdmin, Address sender, string privateKey);
        
        /// <summary>
        /// Remove admin address of the Collection. An admin address can remove itself. List of admins may become empty, in which case only Collection Owner will be able to add an Admin.
        /// </summary>
        /// <param name="removeCollectionAdmin"></param>
        /// <param name="sender"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        string RemoveCollectionAdmin(RemoveCollectionAdmin removeCollectionAdmin, Address sender, string privateKey);

        string SetCollectionSponsor(SetCollectionSponsor setCollectionSponsor, Address sender, string privateKey);

        string ConfirmSponsorship(ulong collectionId, Address sender, string privateKey);

        /// <summary>
        /// Switch back to pay-per-own-transaction model.
        /// </summary>
        /// <param name="removeCollectionSponsor"></param>
        /// <param name="sender"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        string RemoveSponsor(RemoveCollectionSponsor removeCollectionSponsor, Address sender, string privateKey);

        /// <summary>
        /// Set off-chain data schema. In the initial version of NFT parachain the schema will only reflect image URL. The {id} substring will be parsed to reflect the NFT id.
        /// For example, the schema string for CryptoKitties will look like this:
        /// https://img.cryptokitties.co/0x06012c8cf97bead5deae237070f9587f8e7a266d/{id}.png
        /// </summary>
        /// <param name="setOffChainSchema"></param>
        /// <param name="sender"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        string SetOffChainSchema(SetOffChainSchema setOffChainSchema, Address sender, string privateKey);

        /// <summary>
        /// The state variable that stores off-chain data schema for a given collection.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        string? OffChainSchema(ulong collectionId);

        /// <summary>
        /// Non-Fungible Mode: This method is included for compatibility with ERC-721. Return the total count of NFTs of a given Collection that belong to a given address. 
        /// Fungible Mode: Return the amount of token owned by a given address
        /// Re-fungible Mode: Same as non-fungible, but the returned value may be fractional.
        /// </summary>
        /// <param name="getBalanceOf"></param>
        /// <returns></returns>
        ulong? BalanceOf(GetBalanceOf getBalanceOf);

        AdminList? GetAdminList(ulong collectionId);

        TokensList? AddressTokens(AddressTokens addressTokens);

        ulong? NextCollectionId();
        
        event EventHandler<Created> CollectionCreated;

    }
}
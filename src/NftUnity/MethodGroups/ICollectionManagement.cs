using System;
using NftUnity.Models;
using NftUnity.Models.Events;
using Polkadot.DataStructs;

namespace NftUnity.MethodGroups
{
    /// <summary>
    /// Group of operations for managing collections.
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
        /// <param name="collectionId"></param>
        /// <param name="sender"></param>
        /// <param name="privateKey"></param>
        void DestroyCollection(uint collectionId, Address sender, string privateKey);

        event EventHandler<Created> CollectionCreated;
    }
}
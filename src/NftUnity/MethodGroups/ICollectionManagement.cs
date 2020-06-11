using System;
using NftUnity.Models;
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
        /// <param name="customDataSize">Size of NFT properties data.</param>
        /// <param name="sender">Address of initial owner and admin.</param>
        /// <param name="privateKey">Sender's private key.</param>
        void CreateCollection(uint customDataSize, Address sender, string privateKey);

        /// <summary>
        /// </summary>
        event EventHandler<CollectionCreatedEventArgs> CollectionCreated;
    }
}
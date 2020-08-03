using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace NftUnity.Models.Item
{
    public class CreateItem
    {
        /// <summary>
        /// ID of the collection.
        /// </summary>
        [Serialize(0)]
        public ulong CollectionId;

        /// <summary>
        /// Array of bytes that contains NFT properties. Since NFT Module is agnostic of properties’ meaning, it is treated purely as an array of bytes.
        /// </summary>
        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] Properties = null!;

        [Serialize(2)]
        [AddressConverter]
        public Address Owner = null!;

        public CreateItem()
        {
        }

        public CreateItem(ulong collectionId, byte[] properties, Address owner)
        {
            CollectionId = collectionId;
            Properties = properties;
            Owner = owner;
        }
    }
}
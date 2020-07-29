using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace NftUnity.Models.Calls.Item
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

        public CreateItem()
        {
        }

        public CreateItem(ulong collectionId, byte[] properties)
        {
            CollectionId = collectionId;
            Properties = properties;
        }
    }
}
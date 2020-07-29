using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace NftUnity.Models.Calls.Item
{
    public class CreateItem
    {
        [Serialize(0)]
        public ulong CollectionId;

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
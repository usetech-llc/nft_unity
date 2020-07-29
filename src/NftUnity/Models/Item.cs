using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace NftUnity.Models
{
    public class Item
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize(1)]
        public PublicKey Owner = null!;

        [Serialize(2)]
        [PrefixedArrayConverter]
        public byte[] Data = null!;

        public Item()
        {
        }

        public Item(ulong collectionId, PublicKey owner, byte[] data)
        {
            CollectionId = collectionId;
            Owner = owner;
            Data = data;
        }
    }
}
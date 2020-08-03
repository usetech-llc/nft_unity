using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace NftUnity.Models.Item
{
    public class FungibleItem
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize(1)]
        public PublicKey Owner = null!;

        [Serialize(2)]
        [PrefixedArrayConverter]
        public ulong[] Data = null!;

        public FungibleItem()
        {
        }

        public FungibleItem(ulong collectionId, PublicKey owner, ulong[] data)
        {
            CollectionId = collectionId;
            Owner = owner;
            Data = data;
        }
    }
}
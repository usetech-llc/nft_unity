using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace NftUnity.Models.Item
{
    public class ReFungibleItem
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize(1)]
        public PublicKey Owner = null!;

        public ReFungibleItem()
        {
        }

        public ReFungibleItem(ulong collectionId, PublicKey owner)
        {
            CollectionId = collectionId;
            Owner = owner;
        }
    }
}
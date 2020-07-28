using Polkadot.BinarySerializer;

namespace NftUnity.Models.Events
{
    public class ItemDestroyed
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize((1))]
        public ulong ItemId;

        public ItemDestroyed()
        {
        }

        public ItemDestroyed(ulong collectionId, ulong itemId)
        {
            CollectionId = collectionId;
            ItemId = itemId;
        }
    }
}
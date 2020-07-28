using Polkadot.BinaryContracts.Events;
using Polkadot.BinarySerializer;

namespace NftUnity.Models.Events
{
    public class ItemCreated : IEvent
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize(1)]
        public ulong ItemId;

        public ItemCreated()
        {
        }

        public ItemCreated(ulong collectionId, ulong itemId)
        {
            CollectionId = collectionId;
            ItemId = itemId;
        }
    }
}
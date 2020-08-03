using NftUnity.Models.Item;
using Polkadot.BinaryContracts.Events;
using Polkadot.BinarySerializer;

namespace NftUnity.Models.Events
{
    public class ItemCreated : IEvent
    {
        [Serialize(0)]
        public ItemKey Key = null!;

        public ItemCreated()
        {
        }

        public ItemCreated(ItemKey key)
        {
            Key = key;
        }
    }
}
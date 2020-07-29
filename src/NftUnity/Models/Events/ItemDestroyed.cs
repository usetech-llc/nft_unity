using NftUnity.Models.Calls.Item;
using Polkadot.BinarySerializer;

namespace NftUnity.Models.Events
{
    public class ItemDestroyed : IEvent
    {
        [Serialize(0)]
        public ItemKey Key = null!;

        public ItemDestroyed()
        {
        }

        public ItemDestroyed(ItemKey key)
        {
            Key = key;
        }
    }
}
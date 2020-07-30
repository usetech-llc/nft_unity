using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models.Calls.Item
{
    public class Transfer
    {
        [Serialize(0)]
        public ItemKey Key = null!;
        [Serialize(1)]
        [AddressConverter]
        public Address NewOwner = null!;

        public Transfer()
        {
        }

        public Transfer(ItemKey key, Address newOwner)
        {
            Key = key;
            NewOwner = newOwner;
        }
    }
}
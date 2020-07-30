using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models.Calls.Item
{
    public class Approve
    {
        [Serialize(0)]
        [AddressConverter]
        public Address Approved = null!;

        [Serialize(1)]
        public ItemKey Key = null!;

        public Approve()
        {
        }

        public Approve(Address approved, ItemKey key)
        {
            Approved = approved;
            Key = key;
        }
    }
}
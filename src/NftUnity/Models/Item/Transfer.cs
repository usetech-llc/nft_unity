using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models.Item
{
    public class Transfer
    {
        [Serialize(0)]
        [AddressConverter]
        public Address Recipient = null!;
        
        [Serialize(1)]
        public ItemKey Key = null!;

        [Serialize(2)]
        public ulong Value;

        public Transfer()
        {
        }

        public Transfer(Address recipient, ItemKey key, ulong value)
        {
            Key = key;
            Recipient = recipient;
            Value = value;
        }
    }
}
using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models.Item
{
    public class TransferFrom
    {
        [Serialize(0)]
        [AddressConverter]
        public Address From;

        [Serialize(1)]
        [AddressConverter]
        public Address Recipient;

        [Serialize(2)]
        public ItemKey Key;

        [Serialize(3)]
        public ulong Value;

        public TransferFrom()
        {
        }

        public TransferFrom(Address @from, Address recipient, ItemKey key, ulong value)
        {
            From = @from;
            Recipient = recipient;
            Key = key;
            Value = value;
        }
    }
}
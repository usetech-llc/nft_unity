using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models.Item
{
    public class TransferFrom
    {
        [Serialize(0)]
        [AddressConverter]
        public Address Recipient = null!;

        [Serialize(1)]
        public ItemKey Key = null!;

        public TransferFrom()
        {
        }

        public TransferFrom(Address recipient, ItemKey key)
        {
            Key = key;
            Recipient = recipient;
        }
    }
}
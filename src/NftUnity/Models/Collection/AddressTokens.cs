using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models.Collection
{
    public class AddressTokens
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize(1)]
        [AddressConverter]
        public Address Owner = null!;

        public AddressTokens()
        {
        }

        public AddressTokens(ulong collectionId, Address owner)
        {
            CollectionId = collectionId;
            Owner = owner;
        }
    }
}
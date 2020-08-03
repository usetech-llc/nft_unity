using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models.Collection
{
    public class SetCollectionSponsor
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize(1)]
        [AddressConverter]
        public Address NewSponsor = null!;

        public SetCollectionSponsor()
        {
        }

        public SetCollectionSponsor(ulong collectionId, Address newSponsor)
        {
            CollectionId = collectionId;
            NewSponsor = newSponsor;
        }
    }
}
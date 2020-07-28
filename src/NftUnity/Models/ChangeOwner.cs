using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models
{
    public class ChangeOwner
    {
        [Serialize(0)]
        public ulong CollectionId { get; set; }

        [Serialize(1)]
        [Converter(ConverterType = typeof(AddressConverter))]
        public Address NewOwner { get; set; }

        public ChangeOwner()
        {
        }

        public ChangeOwner(ulong collectionId, Address newOwner)
        {
            CollectionId = collectionId;
            NewOwner = newOwner;
        }
    }
}
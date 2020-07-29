using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models.Calls.Collection
{
    public class AddCollectionAdmin
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize(1)]
        [Converter(ConverterType = typeof(AddressConverter))]
        public Address NewAdmin;

        public AddCollectionAdmin()
        {
        }

        public AddCollectionAdmin(ulong collectionId, Address newAdmin)
        {
            CollectionId = collectionId;
            NewAdmin = newAdmin;
        }
    }
}
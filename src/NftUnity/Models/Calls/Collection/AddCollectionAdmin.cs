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
        [AddressConverter]
        public Address NewAdmin = null!;

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
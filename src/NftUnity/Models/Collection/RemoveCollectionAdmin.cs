using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models.Collection
{
    public class RemoveCollectionAdmin
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize(1)]
        [AddressConverter]
        public Address Account = null!;

        public RemoveCollectionAdmin()
        {
        }

        public RemoveCollectionAdmin(ulong collectionId, Address account)
        {
            CollectionId = collectionId;
            Account = account;
        }
    }
}
using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models.Calls.Collection
{
    public class RemoveCollectionAdmin
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize(1)]
        [Converter(ConverterType = typeof(AddressConverter))]
        public Address Account;

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
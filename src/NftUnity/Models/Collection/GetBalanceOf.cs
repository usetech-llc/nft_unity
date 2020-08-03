using System.Linq;
using NftUnity.Converters;
using Polkadot.BinaryContracts;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.Utils;

namespace NftUnity.Models.Collection
{
    public class GetBalanceOf
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize(1)]
        [AddressConverter]
        public Address Account = null!;

        public GetBalanceOf()
        {
        }

        public GetBalanceOf(ulong collectionId, Address account)
        {
            CollectionId = collectionId;
            Account = account;
        }
    }
}
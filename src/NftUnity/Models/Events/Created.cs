using Polkadot.BinaryContracts.Events;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models.Events
{
    public class Created : IEvent
    {
        [Serialize(0)]
        public ulong Id;

        [Serialize(1)]
        public PublicKey Account;

        public Created()
        {
        }

        public Created(ulong id, PublicKey account)
        {
            Id = id;
            Account = account;
        }
    }
}
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace NftUnity.Models.Calls.Collection
{
    public class AdminList
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public PublicKey[] Admins = null!;

        public AdminList()
        {
        }

        public AdminList(PublicKey[] admins)
        {
            Admins = admins;
        }
    }
}
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace NftUnity.Models.Calls.Item
{
    public class ApprovedList
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public PublicKey[] ApprovedAccounts = null!;

        public ApprovedList()
        {
        }

        public ApprovedList(PublicKey[] approvedAccounts)
        {
            ApprovedAccounts = approvedAccounts;
        }
    }
}
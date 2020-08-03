using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace NftUnity.Models.Collection
{
    public class TokensList
    {
        [Serialize(0)]
        [PrefixedArrayConverter]
        public ulong[] TokenIds = null!;

        public TokensList()
        {
        }

        public TokensList(ulong[] tokenIds)
        {
            TokenIds = tokenIds;
        }
    }
}
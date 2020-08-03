using OneOf;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace NftUnity.Models.Collection.CollectionModeEnum
{
    public class CollectionMode
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<Invalid, Nft, Fungible, ReFungible> Mode;

        public CollectionMode()
        {
        }

        public CollectionMode(OneOf<Invalid, Nft, Fungible, ReFungible> mode)
        {
            Mode = mode;
        }
    }
}
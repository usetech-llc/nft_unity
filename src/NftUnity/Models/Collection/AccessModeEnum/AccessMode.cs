using OneOf;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace NftUnity.Models.Collection.AccessModeEnum
{
    public class AccessMode
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<Normal, WhiteList> Mode;

        public AccessMode()
        {
        }

        public AccessMode(OneOf<Normal, WhiteList> mode)
        {
            Mode = mode;
        }
    }
}
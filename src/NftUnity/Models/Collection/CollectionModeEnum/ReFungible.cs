using Polkadot.BinarySerializer;

namespace NftUnity.Models.Collection.CollectionModeEnum
{
    public class ReFungible
    {
        [Serialize(0)]
        public uint DataSize;

        [Serialize(1)]
        public uint DecimalPoints;

        public ReFungible()
        {
        }

        public ReFungible(uint dataSize, uint decimalPoints)
        {
            DataSize = dataSize;
            DecimalPoints = decimalPoints;
        }
    }
}
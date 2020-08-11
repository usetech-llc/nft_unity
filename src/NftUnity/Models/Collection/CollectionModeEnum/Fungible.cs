using Polkadot.BinarySerializer;

namespace NftUnity.Models.Collection.CollectionModeEnum
{
    public class Fungible
    {
        [Serialize(0)]
        public uint DecimalPoints;
        
        public Fungible()
        {
        }

        public Fungible(uint decimalPoints)
        {
            DecimalPoints = decimalPoints;
        }
    }
}
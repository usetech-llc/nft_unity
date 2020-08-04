using Polkadot.BinarySerializer;

namespace NftUnity.Models.Collection.CollectionModeEnum
{
    public class Fungible
    {
        [Serialize(0)]
        public uint Amount;
        
        public Fungible()
        {
        }
    }
}
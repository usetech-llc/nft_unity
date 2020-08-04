using Polkadot.BinarySerializer;

namespace NftUnity.Models.Collection.CollectionModeEnum
{
    public class Nft
    {
        [Serialize(0)]
        public uint CustomDataSize;
    
        public Nft()
        {
        }

        public Nft(uint customDataSize)
        {
            CustomDataSize = customDataSize;
        }
    }
}
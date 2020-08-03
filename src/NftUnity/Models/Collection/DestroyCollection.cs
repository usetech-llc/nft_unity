using Polkadot.BinarySerializer;

namespace NftUnity.Models.Collection
{
    public class DestroyCollection
    {
        [Serialize(0)]
        public ulong CollectionId;

        public DestroyCollection()
        {
        }

        public DestroyCollection(ulong collectionId)
        {
            CollectionId = collectionId;
        }
    }
}
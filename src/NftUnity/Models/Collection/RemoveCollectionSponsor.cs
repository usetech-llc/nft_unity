using Polkadot.BinarySerializer;

namespace NftUnity.Models.Collection
{
    public class RemoveCollectionSponsor
    {
        [Serialize(0)]
        public ulong CollectionId;

        public RemoveCollectionSponsor()
        {
        }

        public RemoveCollectionSponsor(ulong collectionId)
        {
            CollectionId = collectionId;
        }
    }
}
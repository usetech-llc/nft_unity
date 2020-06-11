using System;

namespace NftUnity.Models
{
    public class CollectionCreatedEventArgs : EventArgs
    {
        public string CollectionId { get; }
        public string Owner { get; }

        public CollectionCreatedEventArgs(string collectionId, string owner)
        {
            CollectionId = collectionId;
            Owner = owner;
        }
    }
}
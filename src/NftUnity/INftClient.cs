using System;
using NftUnity.MethodGroups;

namespace NftUnity
{
    public interface INftClient : IDisposable
    {
        ICollectionManagement CollectionManagement { get; }
    }
}
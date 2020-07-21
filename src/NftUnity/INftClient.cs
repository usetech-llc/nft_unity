using System;
using NftUnity.MethodGroups;
using Polkadot.Api;

namespace NftUnity
{
    public interface INftClient : IDisposable
    {
        ICollectionManagement CollectionManagement { get; }
        NftClientSettings Settings { get; }  

        IApplication GetApplication();

        void Connect();
    }
}
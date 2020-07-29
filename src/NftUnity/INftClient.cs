using System;
using NftUnity.MethodGroups;
using Polkadot.Api;
using Polkadot.BinarySerializer;

namespace NftUnity
{
    public interface INftClient : IDisposable
    {
        ICollectionManagement CollectionManagement { get; }
        IItemManagement ItemManagement { get; }
        NftClientSettings Settings { get; }  

        IApplication GetApplication();

        void Connect();

        event EventHandler<IEvent> NewEvent;
        event EventHandler<Exception> OnError;
    }
}
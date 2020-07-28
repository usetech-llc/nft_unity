using System;
using NftUnity.MethodGroups;
using Polkadot.Api;
using Polkadot.BinaryContracts.Events;
using Polkadot.Data;

namespace NftUnity
{
    public interface INftClient : IDisposable
    {
        ICollectionManagement CollectionManagement { get; }
        NftClientSettings Settings { get; }  

        IApplication GetApplication();

        void Connect();

        event EventHandler<IEvent> NewEvent;
    }
}
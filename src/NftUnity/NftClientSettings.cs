using System;
using NftUnity.Logging;
using NftUnity.Models.Events;
using Polkadot;
using Polkadot.Api;
using Polkadot.BinarySerializer;

namespace NftUnity
{
    public class NftClientSettings
    {
        private ILogger _logger = new DoNothingLogger();
        public string WsEndpoint { get; set; } = null!;
        /// <summary>
        /// If request doesn't complete in specified timeout then <see cref="System.TimeoutException"/> will be thrown.
        /// Null means no timeout. 
        /// </summary>
        public TimeSpan? RequestsTimeout { get; set; } = null;
        
        public SerializerSettings SerializerSettings { get; set; }

        public ILogger Logger
        {
            get => _logger;
            set => _logger = value ?? new DoNothingLogger();
        }

        public NftClientSettings()
        {
            SerializerSettings = DefaultNftSerializerSettings();
        }

        public NftClientSettings(string wsEndpoint, ILogger? logger = null, TimeSpan? requestsTimeout = null, SerializerSettings? serializerSettings = null)
        {
            WsEndpoint = wsEndpoint;
            SerializerSettings = serializerSettings ?? DefaultNftSerializerSettings();
            Logger = logger!;
            RequestsTimeout = requestsTimeout;
        }

        public static SerializerSettings DefaultNftSerializerSettings()
        {
            return Application
                .DefaultSubstrateSettings()
                
                .AddEvent<Created>("Nft", "Created")
                .AddEvent<ItemCreated>("Nft", "ItemCreated")
                .AddEvent<ItemDestroyed>("Nft", "ItemDestroyed");
        }
    }
}
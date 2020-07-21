using System;
using NftUnity.Logging;
using Polkadot;

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

        public ILogger Logger
        {
            get => _logger;
            set => _logger = value ?? new DoNothingLogger();
        }

        public NftClientSettings()
        {
        }

        public NftClientSettings(string wsEndpoint, ILogger? logger = null, TimeSpan? requestsTimeout = null)
        {
            WsEndpoint = wsEndpoint;
            Logger = logger!;
            RequestsTimeout = requestsTimeout;
        }
    }
}
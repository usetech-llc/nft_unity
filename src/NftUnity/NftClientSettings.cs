using NftUnity.Logging;
using Polkadot;

namespace NftUnity
{
    public class NftClientSettings
    {
        private ILogger _logger = new DoNothingLogger();
        public string WsEndpoint { get; set; } = null!;

        public ILogger Logger
        {
            get => _logger;
            set => _logger = value ?? new DoNothingLogger();
        }

        public NftClientSettings()
        {
        }

        public NftClientSettings(string wsEndpoint, ILogger logger = null)
        {
            WsEndpoint = wsEndpoint;
            Logger = logger;
        }
    }
}
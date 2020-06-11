using Polkadot;

namespace NftUnity.Logging
{
    internal class DoNothingLogger : ILogger
    {
        public void Info(string message)
        {
        }

        public void Error(string message)
        {
        }

        public void Warning(string message)
        {
        }
    }
}
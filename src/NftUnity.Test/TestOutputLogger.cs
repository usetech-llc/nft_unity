using Polkadot;
using Xunit.Abstractions;

namespace NftUnity.Test
{
    public class TestOutputLogger : ILogger
    {
        private readonly ITestOutputHelper _output;

        public TestOutputLogger(ITestOutputHelper output)
        {
            _output = output;
        }
        
        public void Info(string message)
        {
            _output.WriteLine($"INFO: {message}");
        }

        public void Error(string message)
        {
            _output.WriteLine($"ERROR: {message}");
        }

        public void Warning(string message)
        {
            _output.WriteLine($"WARNING: {message}");
        }
    }
}
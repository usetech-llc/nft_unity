using System;
using Polkadot;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NftUnity.Test
{
    public class BaseTest
    {
        public ITestOutputHelper Output { get; }
        public TestConfiguration Configuration { get; }
        
        public BaseTest(ITestOutputHelper output)
        {
            Output = output;
            Configuration = TestConfiguration.Create();
        }

        public INftClient CreateClient()
        {
            return new NftClient(new NftClientSettings(Configuration.WsEndpoint, new TestOutputLogger(Output), TimeSpan.FromSeconds(30)));
        }
    }
}
namespace NftUnity.Test
{
    public class BaseTest
    {
        public TestConfiguration Configuration { get; }
        
        public BaseTest()
        {
            Configuration = TestConfiguration.Create();
        }

        public INftClient CreateClient()
        {
            return new NFtClient(new NftClientSettings(Configuration.WsEndpoint));
        }
    }
}
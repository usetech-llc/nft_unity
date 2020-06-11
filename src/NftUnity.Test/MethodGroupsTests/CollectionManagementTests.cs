using Polkadot.DataStructs;
using Xunit;

namespace NftUnity.Test.MethodGroupsTests
{
    public class CollectionManagementTests : BaseTest
    {
        [Fact]
        public void CreateCollectionEmitsEvent()
        {
            using var client = CreateClient();
            
            client.CollectionManagement.CreateCollection(200,new Address(Configuration.Account1.Address), Configuration.Account1.PrivateKey);
        }
    }
}
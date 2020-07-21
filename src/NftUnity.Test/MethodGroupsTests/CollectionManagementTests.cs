using System;
using System.Numerics;
using System.Threading.Tasks;
using NftUnity.Models;
using NftUnity.Test.Extensions;
using Polkadot.DataStructs;
using Polkadot.Source.Utils;
using Polkadot.Utils;
using Xunit;
using Xunit.Abstractions;

namespace NftUnity.Test.MethodGroupsTests
{
    public class CollectionManagementTests : BaseTest
    {
        public CollectionManagementTests(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact]
        public void CreateCollectionCompletes()
        {
            var collectionName = Guid.NewGuid().ToString("N");
            var collectionDescription = Guid.NewGuid().ToString("N");
            var tokenPrefix = Guid.NewGuid().ToString("N");
            var size = 200u;
            
            var createCollection = new CreateCollection(collectionName, collectionDescription, tokenPrefix, size);
            using var client = CreateClient();

            var creationResult = client.CollectionManagement.CreateCollection(createCollection,new Address(Configuration.Account1.Address), Configuration.Account1.PrivateKey);

            Assert.NotEmpty(creationResult);
        }

        [Fact]
        public void ChangeOwnerCollectionChangesSomething()
        {
            
        }
    }
}
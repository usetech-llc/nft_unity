using System;
using System.Threading.Tasks;
using NftUnity.Models;
using NftUnity.Models.Events;
using Polkadot.DataStructs;
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
        public async Task CreateCollectionEmmitsEvent()
        {
            var collectionName = "1111";
            var collectionDescription = "1111";
            var tokenPrefix = "1111";
            var size = 200u;
            
            var collectionCreatedTask = new TaskCompletionSource<Created>();
            var createCollection = new CreateCollection(collectionName, collectionDescription, tokenPrefix, size);
            using var blockClient = CreateClient();
            blockClient.CollectionManagement.CollectionCreated += (sender, @event) =>
            {
                if (AddressUtils.GetAddrFromPublicKey(@event.Account).Equals(Configuration.Account1.Address))
                {
                    collectionCreatedTask.SetResult(@event);
                }
            };

            using var client = CreateClient();
            client.CollectionManagement.CreateCollection(createCollection,new Address(Configuration.Account1.Address), Configuration.Account1.PrivateKey);

            await collectionCreatedTask.Task
                .WithTimeout(TimeSpan.FromMinutes(1));

            var created = collectionCreatedTask.Task.Result;
            Assert.NotNull(created);
            Output.WriteLine($"Created collection with id: {created.Id}");
        }

        [Fact]
        public void ChangeOwnerCollectionChangesSomething()
        {
            var collectionId = 1UL;
            var changeOwner = new ChangeOwner(collectionId, new Address(Configuration.Account2.Address));

            using var client = CreateClient();

            var changingResult = client.CollectionManagement.ChangeCollectionOwner(changeOwner, new Address(Configuration.Account1.Address), Configuration.Account1.PrivateKey);
            
            Assert.NotEmpty(changingResult);
        }
    }
}
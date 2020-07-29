using System;
using System.Threading.Tasks;
using NftUnity.Models.Calls.Collection;
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
        public async Task CreateCollectionEmitsEvent()
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
        public async Task GetCollectionMatchesNewlyCreatedCollection()
        {
            var name = Guid.NewGuid().ToString("N");
            var description = Guid.NewGuid().ToString("N");
            var prefix = Guid.NewGuid().ToString("N")[..15];
            var size = 9u;
            
            var collectionCreatedTask = new TaskCompletionSource<Created>();
            var createCollection = new CreateCollection(name, description, prefix, size);
            using var blockClient = CreateClient();
            blockClient.CollectionManagement.CollectionCreated += (sender, @event) =>
            {
                if (AddressUtils.GetAddrFromPublicKey(@event.Account).Equals(Configuration.Account1.Address))
                {
                    collectionCreatedTask.SetResult(@event);
                }
            };

            blockClient.CollectionManagement.CreateCollection(createCollection, new Address() {Symbols = Configuration.Account1.Address}, Configuration.Account1.PrivateKey);

            var created = await collectionCreatedTask.Task;

            var collection = blockClient.CollectionManagement.GetCollection(created.Id);
            
            Assert.Equal(name, collection!.Name);
            Assert.Equal(description, collection.Description);
            Assert.Equal(prefix, collection.TokenPrefix);
            Assert.Equal(size, collection.CustomDataSize);
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Account1.Address).Bytes, collection.Owner.Bytes);
        }

        [Fact]
        public async Task ChangeOwnerCollectionChangesOwner()
        {
            using var client = CreateClient();

            var id = await CreateTestAccount1Collection();
            
            var changeCollectionOwner = new ChangeOwner(id, new Address(Configuration.Account2.Address));

            client.CollectionManagement.ChangeCollectionOwner(changeCollectionOwner, new Address(Configuration.Account1.Address), Configuration.Account1.PrivateKey);

            var collection = client.CollectionManagement.GetCollection(id);
            
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Account1.Address).Bytes, collection!.Owner.Bytes);
        }

        [Fact]
        public async Task DestroyingCollectionMakesItDisappear()
        {
            var id = await CreateTestAccount1Collection();

            var client = CreateClient();
            client.CollectionManagement.DestroyCollection(new DestroyCollection(id),
                new Address(Configuration.Account1.Address), Configuration.Account1.PrivateKey);

            await WaitBlocks(10);
           
            var collection = client.CollectionManagement.GetCollection(id);
            Assert.Null(collection);
        }
    }
}
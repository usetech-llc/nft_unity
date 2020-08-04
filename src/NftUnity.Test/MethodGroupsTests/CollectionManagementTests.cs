using System;
using System.Threading.Tasks;
using NftUnity.Models.Collection;
using NftUnity.Models.Collection.CollectionModeEnum;
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
            var mode = new CollectionMode(new Nft(200));

            var collectionCreatedTask = new TaskCompletionSource<Created>();
            var createCollection = new CreateCollection(collectionName, collectionDescription, tokenPrefix, mode);
            using var blockClient = CreateClient();
            blockClient.CollectionManagement.CollectionCreated += (sender, @event) =>
            {
                if (AddressUtils.GetAddrFromPublicKey(@event.Account).Equals(Configuration.Alice.Address))
                {
                    collectionCreatedTask.SetResult(@event);
                }
            };

            using var client = CreateClient();
            client.CollectionManagement.CreateCollection(createCollection,new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

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
            var mode = new CollectionMode(new Nft(size));
            
            var collectionCreatedTask = new TaskCompletionSource<Created>();
            var createCollection = new CreateCollection(name, description, prefix, mode);
            using var blockClient = CreateClient();
            blockClient.CollectionManagement.CollectionCreated += (sender, @event) =>
            {
                if (AddressUtils.GetAddrFromPublicKey(@event.Account).Equals(Configuration.Alice.Address))
                {
                    collectionCreatedTask.SetResult(@event);
                }
            };

            blockClient.CollectionManagement.CreateCollection(createCollection, new Address() {Symbols = Configuration.Alice.Address}, Configuration.Alice.PrivateKey);

            var created = await collectionCreatedTask.Task;

            var collection = blockClient.CollectionManagement.GetCollection(created.Id);
            
            Assert.Equal(name, collection!.Name);
            Assert.Equal(description, collection.Description);
            Assert.Equal(prefix, collection.TokenPrefix);
            Assert.Equal(size, collection.CustomDataSize);
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Alice.Address).Bytes, collection.Owner.Bytes);
        }

        [Fact]
        public async Task ChangeOwnerCollectionChangesOwner()
        {
            using var client = CreateClient();

            var id = await CreateTestAliceCollection();
            
            var changeCollectionOwner = new ChangeOwner(id, new Address(Configuration.Bob.Address));

            client.CollectionManagement.ChangeCollectionOwner(changeCollectionOwner, new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            var collection = client.CollectionManagement.GetCollection(id);
            
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Alice.Address).Bytes, collection!.Owner.Bytes);
        }

        [Fact]
        public async Task DestroyingCollectionMakesItDisappear()
        {
            var id = await CreateTestAliceCollection();

            using var client = CreateClient();
            client.CollectionManagement.DestroyCollection(new DestroyCollection(id),
                new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(10);
           
            var collection = client.CollectionManagement.GetCollection(id);
            Assert.Null(collection);
        }

        [Fact]
        public async Task AddCollectionAdminAddsAccountToAdminList()
        {
            var id = await CreateTestAliceCollection();
            
            var account2PublicKey = AddressUtils.GetPublicKeyFromAddr(Configuration.Bob.Address).Bytes;
            
            using var client = CreateClient();

            var adminList = client.CollectionManagement.GetAdminList(id);
            if (adminList != null)
            {
                Assert.DoesNotContain(adminList.Admins, pk => account2PublicKey == pk.Bytes);
            }

            client.CollectionManagement.AddCollectionAdmin(
                new AddCollectionAdmin(id, new Address(Configuration.Bob.Address)),
                new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(2);

            var adminListAfter = client.CollectionManagement.GetAdminList(id);
            Assert.Contains(adminListAfter!.Admins, pk => pk.Bytes.ToHexString().Equals(account2PublicKey.ToHexString()));
        }

        [Fact]
        public async Task RemoveCollectionAdminRemovesAdminFromList()
        {
            var id = await CreateTestAliceCollection();
            
            var account2PublicKey = AddressUtils.GetPublicKeyFromAddr(Configuration.Bob.Address).Bytes;
            
            using var client = CreateClient();

            client.CollectionManagement.AddCollectionAdmin(
                new AddCollectionAdmin(id, new Address(Configuration.Bob.Address)),
                new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(2);

            var adminList = client.CollectionManagement.GetAdminList(id);
            Assert.Contains(adminList!.Admins, pk => pk.Bytes.ToHexString().Equals(account2PublicKey.ToHexString()));

            client.CollectionManagement.RemoveCollectionAdmin(
                new RemoveCollectionAdmin(id, new Address(Configuration.Bob.Address)),
                new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(2);
            var adminListAfter = client.CollectionManagement.GetAdminList(id);
            if (adminListAfter != null)
            {
                Assert.DoesNotContain(adminListAfter.Admins, pk => account2PublicKey == pk.Bytes);
            }
        }

        [Fact]
        public async Task BalanceOfOwnerIsOne()
        {
            var itemKey = await CreateTestAliceItem();

            var client = CreateClient();
            var balance = client.CollectionManagement.BalanceOf(new GetBalanceOf(itemKey.CollectionId, new Address(Configuration.Alice.Address)));
            Assert.Equal(1UL, balance);
        }

        [Fact]
        public async Task BalanceOfNotOwnerIsZero()
        {
            var itemKey = await CreateTestAliceItem();

            var client = CreateClient();
            var balance = client.CollectionManagement.BalanceOf(new GetBalanceOf(itemKey.CollectionId, new Address(Configuration.Bob.Address)));
            Assert.Equal(0UL, balance ?? 0);
        }

        [Fact]
        public async Task NextIdGreaterOrEqualLastCreatedCollectionId()
        {
            var id = await CreateTestAliceCollection();

            using var client = CreateClient();
            var nextId = client.CollectionManagement.NextCollectionId();
            
            Assert.True(nextId >= id);
        }

        [Fact]
        public async Task SetCollectionSponsorAddsUnconfirmedSponsor()
        {
            var id = await CreateTestAliceCollection();

            using var client = CreateClient();

            client.CollectionManagement.SetCollectionSponsor(
                new SetCollectionSponsor(id, new Address(Configuration.Bob.Address)),
                new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(2);
            var collection = client.CollectionManagement.GetCollection(id);
            
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Bob.Address).Bytes, collection!.UnconfirmedSponsor.Bytes);
            Assert.NotEqual(AddressUtils.GetPublicKeyFromAddr(Configuration.Bob.Address).Bytes, collection!.Sponsor.Bytes);
        }

        [Fact]
        public async Task ConfirmSponsorshipMakesSponsorConfirmed()
        {
            var id = await CreateTestAliceCollection();

            using var client = CreateClient();

            client.CollectionManagement.SetCollectionSponsor(
                new SetCollectionSponsor(id, new Address(Configuration.Bob.Address)),
                new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(2);

            client.CollectionManagement.ConfirmSponsorship(id, new Address(Configuration.Bob.Address), Configuration.Bob.PrivateKey);
            
            await WaitBlocks(2);

            var collection = client.CollectionManagement.GetCollection(id);
            
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Bob.Address).Bytes, collection!.Sponsor.Bytes);
            Assert.NotEqual(AddressUtils.GetPublicKeyFromAddr(Configuration.Bob.Address).Bytes, collection.UnconfirmedSponsor.Bytes);
        }

        [Fact]
        public async Task AddressTokensReturnsOwnedToken()
        {
            var itemKey = await CreateTestAliceItem();

            using var client = CreateClient();

            var tokens = client.CollectionManagement.AddressTokens(new AddressTokens(itemKey.CollectionId, new Address(Configuration.Alice.Address)));

            Assert.Contains(tokens!.TokenIds, id => id == itemKey.ItemId);
        }
        
        
        [Fact]
        public async Task RemoveSponsorRemovesUnconfirmedSponsor()
        {
            var id = await CreateTestAliceCollection();

            using var client = CreateClient();

            client.CollectionManagement.SetCollectionSponsor(
                new SetCollectionSponsor(id, new Address(Configuration.Bob.Address)),
                new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(2);

            client.CollectionManagement.ConfirmSponsorship(id, new Address(Configuration.Bob.Address), Configuration.Bob.PrivateKey);
            
            await WaitBlocks(2);

            client.CollectionManagement.RemoveSponsor(new RemoveCollectionSponsor(id), new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(2);
            
            var collection = client.CollectionManagement.GetCollection(id);
            
            Assert.All(collection!.UnconfirmedSponsor.Bytes, b => Assert.Equal(0, b));
            Assert.All(collection.Sponsor.Bytes, b => Assert.Equal(0, b));
        }

        [Fact]
        public async Task OffChainSchemaMatchesPreviouslySetSchema()
        {
            var schema = Guid.NewGuid().ToString("N");
            var id = await CreateTestAliceCollection();

            using var client = CreateClient();
            client.CollectionManagement.SetOffChainSchema(new SetOffChainSchema(id, schema), new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(2);
            
            var storedSchema = client.CollectionManagement.OffChainSchema(id);
            
            Assert.Equal(schema, storedSchema);
        }
    }
}
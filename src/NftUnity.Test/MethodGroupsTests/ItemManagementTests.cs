using System;
using System.Threading.Tasks;
using NftUnity.Models.Calls.Collection;
using NftUnity.Models.Calls.Item;
using NftUnity.Models.Events;
using Polkadot.DataStructs;
using Polkadot.Utils;
using Xunit;
using Xunit.Abstractions;

namespace NftUnity.Test.MethodGroupsTests
{
    public class ItemManagementTests : BaseTest
    {
        public ItemManagementTests(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact(Skip = "Need to figure out how to make correct ItemList storage request.")]
        public async Task CreatedItemMatchesGetItem()
        {
            var collectionId = await CreateTestAccount1Collection();
            var properties = Guid.NewGuid().ToByteArray();
            
            var createItem = new CreateItem(collectionId, properties);
            using var client = CreateClient();
            
            var itemCreatedTask = new TaskCompletionSource<ItemCreated>();
            client.ItemManagement.ItemCreated += (sender, created) =>
            {
                if (created.Key.CollectionId == collectionId)
                {
                    itemCreatedTask.SetResult(created);
                }
            };

            client.ItemManagement.CreateItem(createItem, new Address(Configuration.Account1.Address), Configuration.Account1.PrivateKey);

            var key = (await itemCreatedTask.Task.WithTimeout(TimeSpan.FromSeconds(30))).Key;

            var item = client.ItemManagement.GetItem(key);
            
            Assert.Equal(properties, item!.Data);
            Assert.Equal(collectionId, item!.CollectionId);
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Account1.Address).Bytes, item!.Owner.Bytes);
        }

        [Fact]
        public async Task BurnItemEmitsEvent()
        {
            var itemKey = await CreateTestAccount1Item();

            using var client = CreateClient();
            var destroyedTask = new TaskCompletionSource<ItemDestroyed>();
            client.ItemManagement.ItemDestroyed += (sender, destroyed) =>
            {
                if (destroyed.Key.CollectionId == itemKey.CollectionId && destroyed.Key.ItemId == itemKey.ItemId)
                {
                    destroyedTask.SetResult(destroyed);
                }
            };

            client.ItemManagement.BurnItem(itemKey, new Address(Configuration.Account1.Address), Configuration.Account1.PrivateKey);
            var destroyedResult = await destroyedTask.Task.WithTimeout(TimeSpan.FromSeconds(30));
            
            Assert.NotNull(destroyedResult);
        }
    }
}
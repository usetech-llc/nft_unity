using System;
using System.Threading.Tasks;
using NftUnity.MethodGroups;
using NftUnity.Models.Events;
using NftUnity.Models.Item;
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
        
        [Fact]
        public async Task CreatedItemMatchesGetItem()
        {
            var bytes = new byte[] {15, 97, 136, 0, 76, 187, 168, 28, 239, 85, 170, 23, 77, 81, 248, 159,};
            var str = bytes.ToHexString();
                
            var collectionId = await CreateTestAliceCollection();
            var properties = Guid.NewGuid().ToByteArray();
            
            var createItem = new CreateItem(collectionId, properties, new Address(Configuration.Alice.Address));
            using var client = CreateClient();
            
            var itemCreatedTask = new TaskCompletionSource<ItemCreated>();
            client.ItemManagement.ItemCreated += (sender, created) =>
            {
                if (created.Key.CollectionId == collectionId)
                {
                    itemCreatedTask.SetResult(created);
                }
            };

            client.ItemManagement.CreateItem(createItem, new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            var key = (await itemCreatedTask.Task.WithTimeout(TimeSpan.FromSeconds(30))).Key;

            var item = client.ItemManagement.GetNftItem(key);
            
            Assert.Equal(properties, item!.Data);
            Assert.Equal(collectionId, item!.CollectionId);
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Alice.Address).Bytes, item!.Owner.Bytes);
        }

        [Fact]
        public async Task BurnItemEmitsEvent()
        {
            var itemKey = await CreateTestAliceItem();

            using var client = CreateClient();
            var destroyedTask = new TaskCompletionSource<ItemDestroyed>();
            client.ItemManagement.ItemDestroyed += (sender, destroyed) =>
            {
                if (destroyed.Key.CollectionId == itemKey.CollectionId && destroyed.Key.ItemId == itemKey.ItemId)
                {
                    destroyedTask.SetResult(destroyed);
                }
            };

            client.ItemManagement.BurnItem(itemKey, new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);
            var destroyedResult = await destroyedTask.Task.WithTimeout(TimeSpan.FromSeconds(30));
            
            Assert.NotNull(destroyedResult);
        }

        [Fact]
        public async Task TransferChangesOwner()
        {
            var key = await CreateTestAliceItem();

            using var client = CreateClient();
            client.ItemManagement.Transfer(new Transfer(new Address(Configuration.Bob.Address), key, 0),
                new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(2);

            var item = client.ItemManagement.GetNftItem(key);
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Bob.Address).Bytes, item?.Owner.Bytes);
        }

        [Fact]
        public async Task ApproveAddsAccountToApprovedList()
        {
            var key = await CreateTestAliceItem();

            using var client = CreateClient();
            var publicKey2 = AddressUtils.GetPublicKeyFromAddr(Configuration.Bob.Address);
            var approveListBefore = client.ItemManagement.GetApproved(key);
            if (approveListBefore != null)
            {
                Assert.DoesNotContain(approveListBefore!.ApprovedAccounts, a => a.Bytes.ToHexString().Equals(publicKey2.Bytes.ToHexString()));
            }

            client.ItemManagement.Approve(new Approve(new Address(Configuration.Bob.Address), key),
                new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(2);

            var approveList = client.ItemManagement.GetApproved(key);
            Assert.Contains(approveList!.ApprovedAccounts, a => a.Bytes.ToHexString().Equals(publicKey2.Bytes.ToHexString()));
        }

        [Fact(Skip = "Not part of the api.")]
        public async Task NextItemIdGreaterOrEqualToLastCreatedItemId()
        {
            var key = await CreateTestAliceItem();

            using var client = CreateClient();
            var nextId = ((ItemManagement)client.ItemManagement).NextId(key.CollectionId);
            
            Assert.True(nextId >= key.ItemId);
        }

        [Fact]
        public async Task GetOwnerReturnsItCreatorAfterCreation()
        {
            var key = await CreateTestAliceItem();

            using var client = CreateClient();
            var owner = client.ItemManagement.GetOwner(key);
            
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Alice.Address).Bytes, owner!.Bytes);
        }

        [Fact(Skip = "Backend errors.")]
        public async Task TransferFromChangesOwnerIfMadeByApprovedAccount()
        {
            var key = await CreateTestAliceItem();

            using var client = CreateClient();
            client.ItemManagement.Approve(new Approve(new Address(Configuration.Bob.Address), key),
                new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);

            await WaitBlocks(2);

            client.ItemManagement.TransferFrom(new TransferFrom(new Address(Configuration.Alice.Address), new Address(Configuration.Charlie.Address), key, 0),
                new Address(Configuration.Bob.Address), Configuration.Bob.PrivateKey);

            await WaitBlocks(2);
            
            var item = client.ItemManagement.GetNftItem(key);
            
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Charlie.Address).Bytes, item!.Owner.Bytes);

            var approvedList = client.ItemManagement.GetApproved(key);

            if (approvedList != null)
            {
                Assert.DoesNotContain(approvedList.ApprovedAccounts, a => a.Bytes.ToHexString().Equals(AddressUtils.GetPublicKeyFromAddr(Configuration.Bob.Address).Bytes.ToHexString()));
            }
        }

        [Fact]
        public async Task TransferFromFailsForNotApprovedAccount()
        {
            var key = await CreateTestAliceItem();

            using var client = CreateClient();
            client.ItemManagement.TransferFrom(new TransferFrom(new Address(Configuration.Alice.Address), new Address(Configuration.Charlie.Address), key, 0), new Address(Configuration.Bob.Address), Configuration.Bob.PrivateKey);

            await WaitBlocks(2);

            var item = client.ItemManagement.GetNftItem(key);
            
            Assert.Equal(AddressUtils.GetPublicKeyFromAddr(Configuration.Alice.Address).Bytes, item!.Owner.Bytes);
        }
    }
}
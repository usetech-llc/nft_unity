using System;
using System.Threading.Tasks;
using NftUnity.Extensions;
using NftUnity.Models.Collection;
using NftUnity.Models.Collection.CollectionModeEnum;
using NftUnity.Models.Events;
using NftUnity.Models.Item;
using Polkadot;
using Polkadot.DataStructs;
using Polkadot.Utils;
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


        public async Task<ulong> CreateTestAliceCollection(CollectionMode? mode = null)
        {
            mode ??= new CollectionMode(new Nft(256));
            var name = Guid.NewGuid().ToString("N");
            var description = Guid.NewGuid().ToString("N");
            var prefix = Guid.NewGuid().ToString("N")[..15];
            var createCollection = new CreateCollection(name, description, prefix, mode);
            
            var collectionCreatedTask = new TaskCompletionSource<Created>();
            using var blockClient = CreateClient();
            blockClient.CollectionManagement.CollectionCreated += (sender, @event) =>
            {
                if (AddressUtils.GetAddrFromPublicKey(@event.Account).Equals(Configuration.Alice.Address))
                {
                    collectionCreatedTask.SetResult(@event);
                }
            };

            blockClient.CollectionManagement.CreateCollection(createCollection, new Address() {Symbols = Configuration.Alice.Address}, Configuration.Alice.PrivateKey);

            var created = await collectionCreatedTask.Task.WithTimeout(TimeSpan.FromSeconds(30));
            return created.Id;
        }

        public async Task<ItemKey> CreateTestAliceItem(CollectionMode? mode = null)
        {
            var collectionId = await CreateTestAliceCollection(mode);
            
            var keyTask = new TaskCompletionSource<ItemKey>();

            using var client = CreateClient();
            client.ItemManagement.ItemCreated += (sender, created) =>
            {
                if (created.Key.CollectionId == collectionId)
                {
                    keyTask.SetResult(created.Key);
                }
            };

            client.ItemManagement.CreateItem(new CreateItem(collectionId, Guid.NewGuid().ToByteArray(), new Address(Configuration.Alice.Address)),
                new Address(Configuration.Alice.Address), Configuration.Alice.PrivateKey);
            
            return await keyTask.Task;
        }

        public async Task WaitBlocks(int blocksCount)
        {
            var nextBlock = new TaskCompletionSource<long>();
            var client = CreateClient();
            client.MakeCallWithReconnect(app =>
            {
                app.SubscribeBlockNumber(block =>
                {
                    if (blocksCount-- > 0)
                    {
                        return;
                    }
                    nextBlock.SetResult(block);
                });
            });

            await nextBlock.Task;
        }
    }
}
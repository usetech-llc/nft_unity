using System;
using System.Threading.Tasks;
using NftUnity.Extensions;
using NftUnity.Models.Calls.Collection;
using NftUnity.Models.Events;
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


        public async Task<ulong> CreateTestAccount1Collection()
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
            return created.Id;
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
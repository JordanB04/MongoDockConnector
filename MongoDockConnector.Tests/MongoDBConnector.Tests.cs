using System.Threading.Tasks;
using MongoDockConnector.Lib;
using Testcontainers.MongoDb;
using Xunit;

namespace MongoDockConnector.Tests
{
    public class MongoDBConnector_SuccessTest : IAsyncLifetime
    {
        private MongoDbContainer _mongo;

        public async Task InitializeAsync()
        {
            _mongo = new MongoDbBuilder()
                .WithImage("mongo:7")
                .Build();
            await _mongo.StartAsync();
        }

        public async Task DisposeAsync()
        {
            if (_mongo is not null)
            {
                await _mongo.DisposeAsync();
            }
        }

        [Fact]
        public async Task PingAsync_ReturnsTrue_WhenMongoIsReachable()
        {
            var connector = new MongoDBConnector(_mongo.GetConnectionString());
            var ok = await connector.PingAsync();
            Assert.True(ok);
        }
    }
}

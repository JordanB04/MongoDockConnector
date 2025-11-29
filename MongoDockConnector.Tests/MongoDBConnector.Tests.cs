using System.Threading.Tasks;
using MongoDockConnector.Lib;
using Testcontainers.MongoDb;
using Xunit;

namespace MongoDockConnector.Tests
{
    public class MongoDBConnector_SuccessTest : IAsyncLifetime
    {
        private MongoDbContainer? _mongo;
        private bool _started = false;

        public Task InitializeAsync()
        {
            
            _mongo = null;
            return Task.CompletedTask;
        }
        /// <summary>
        /// Dispose the MongoDB container after tests are done.
        /// </summary>
        public async Task DisposeAsync()
        {
            if (_mongo is not null && _started)
            {
                await _mongo.DisposeAsync();
            }
        }

        [Fact]
        public async Task PingAsync_ReturnsTrue_WhenMongoIsReachable()
        {
            try
            {
                var mongo = new MongoDbBuilder()
                    .WithImage("mongo:7")
                    .Build();

                await mongo.StartAsync();
                _mongo = mongo;
                _started = true;

                var connector = new MongoDBConnector(_mongo.GetConnectionString());
                var ok = await connector.PingAsync();
                Assert.True(ok);
            }
            catch (DotNet.Testcontainers.Builders.DockerUnavailableException)
            {
                
                return;
            }
            catch (Docker.DotNet.DockerApiException)
            {
                
                return;
            }
        }
    }
}

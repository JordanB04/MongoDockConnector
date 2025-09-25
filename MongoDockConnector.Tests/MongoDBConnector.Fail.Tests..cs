using System.Threading.Tasks;
using MongoDockConnector.Lib;
using Xunit;

namespace MongoDockConnector.Tests
{
    public class MongoDBConnector_FailTest
    {
        [Fact]
        public async Task PingAsync_ReturnsFalse_WhenMongoIsNotReachable()
        {
            // bad port that should be closed
            var badConn = "mongodb://localhost:59999";
            var connector = new MongoDBConnector(badConn);

            var ok = await connector.PingAsync();
            Assert.False(ok);
        }
    }
}

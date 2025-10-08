using System.Threading.Tasks;
using MongoDockConnector.Lib;
using Testcontainers.PostgreSql;
using Xunit;

namespace MongoDockConnector.Tests
{
    public class PostgresConnector_SuccessTest : IAsyncLifetime
    {
        private PostgreSqlContainer _pg;

        public async Task InitializeAsync()
        {
            _pg = new PostgreSqlBuilder()
                .WithImage("postgres:16")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .WithDatabase("postgres")
                .Build();

            await _pg.StartAsync();
        }

        public async Task DisposeAsync()
        {
            if (_pg is not null)
            {
                await _pg.DisposeAsync();
            }
        }

        [Fact]
        public async Task PingAsync_ReturnsTrue_WhenPostgresIsReachable()
        {
            var connStr = _pg.GetConnectionString(); // includes Host, Port, Username, Password, Database
            IDBConnector connector = new PostgresConnector(connStr);
            var ok = await connector.PingAsync();
            Assert.True(ok);
        }
    }
}

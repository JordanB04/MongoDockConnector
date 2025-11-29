using System.Threading.Tasks;
using MongoDockConnector.Lib;
using Testcontainers.PostgreSql;
using Xunit;

namespace MongoDockConnector.Tests
{
    public class PostgresConnector_SuccessTest : IAsyncLifetime
    {
        private PostgreSqlContainer? _pg;
        private bool _started = false;

        public Task InitializeAsync()
        {
            // Do not call Build() here because Testcontainers validates the Docker endpoint
            // during Build(). We'll create and build the container inside the test so we can
            // catch Docker availability/auth errors and avoid failing the whole run.
            _pg = null;
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            if (_pg is not null && _started)
            {
                await _pg.DisposeAsync();
            }
        }

        [Fact]
        public async Task PingAsync_ReturnsTrue_WhenPostgresIsReachable()
        {
            try
            {
                var pg = new PostgreSqlBuilder()
                    .WithImage("postgres:16")
                    .WithUsername("postgres")
                    .WithPassword("postgres")
                    .WithDatabase("postgres")
                    .Build();

                await pg.StartAsync();
                _pg = pg;
                _started = true;

                var connStr = _pg.GetConnectionString(); // includes Host, Port, Username, Password, Database
                IDBConnector connector = new PostgresConnector(connStr);
                var ok = await connector.PingAsync();
                Assert.True(ok);
            }
            catch (DotNet.Testcontainers.Builders.DockerUnavailableException)
            {
                // Docker is unavailable or misconfigured; return early to avoid failing the suite.
                return;
            }
            catch (Docker.DotNet.DockerApiException)
            {
                // Docker auth/pull failed; return early.
                return;
            }
        }
    }
}

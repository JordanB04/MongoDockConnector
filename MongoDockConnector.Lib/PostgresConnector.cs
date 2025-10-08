using Npgsql;
using System;
using System.Threading.Tasks;

namespace MongoDockConnector.Lib
{
    /// <summary>
    /// Simple PostgreSQL connector implementing IDBConnector.
    /// </summary>
    public class PostgresConnector : IDBConnector
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initialize with a standard Postgres connection string.
        /// Example: Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres
        /// </summary>
        public PostgresConnector(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Pings PostgreSQL by opening a connection and executing SELECT 1.
        /// </summary>
        public async Task<bool> PingAsync()
        {
            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();
                await using var cmd = new NpgsqlCommand("SELECT 1;", conn);
                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result) == 1;
            }
            catch
            {
                return false;
            }
        }
    }
}

using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MongoDockConnector.Lib
{
    public class MongoDBConnector
    {
        private readonly string _connectionString;
        private readonly MongoClient _client;

        /// <summary>
        /// Initializes the connector with a MongoDB connection string.
        /// </summary>
        /// <param name="connectionString">MongoDB connection string.</param>
        /// <exception cref="ArgumentNullException">Thrown when connectionString is null.</exception>
        /// </summary>
        public MongoDBConnector(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _client = new MongoClient(_connectionString);
        }

        /// <summary>
        /// Pings the MongoDB server by running { ping: 1 } against the admin DB.
        /// Returns true on success, false on any exception. Accepts no parameters.
        /// <returns>True if the ping command succeeds, false otherwise.</returns>
        /// </summary>
        public async Task<bool> PingAsync()
        {
            try
            {
                var db = _client.GetDatabase("admin");
                await db.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

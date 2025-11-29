using System;
using System.Threading.Tasks;
using MongoDockConnector.Lib;

namespace DBConnectorReplApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== DB Connector REPL ===");
            Console.WriteLine("Type 'exit' to quit.\n");

            while (true)
            {
                Console.Write("Choose a database (mongo/postgres): ");
                string db = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

                if (db == "exit")
                    break;

                if (db != "mongo" && db != "postgres")
                {
                    Console.WriteLine("Invalid option.\n");
                    continue;
                }

                Console.Write("Enter connection string: ");
                string conn = Console.ReadLine()?.Trim() ?? string.Empty;

                if (conn == "exit")
                    break;

                IDBConnector connector;

                if (db == "mongo")
                    connector = new MongoDBConnector(conn);
                else
                    connector = new PostgresConnector(conn);

                Console.WriteLine("Pinging database...");

                try
                {
                    bool ok = await connector.PingAsync();

                    if (ok)
                        Console.WriteLine("✅ Connected successfully!\n");
                    else
                        Console.WriteLine("❌ Connection failed.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}\n");
                }
            }

            Console.WriteLine("Exiting...");
        }
    }
}

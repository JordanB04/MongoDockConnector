using System.Threading.Tasks;

namespace MongoDockConnector.Lib
{
    /// <summary>
    /// Minimal DB connector abstraction for health checks.
    /// </summary>
    public interface IDBConnector
    {
        /// <summary>
        /// Pings the backing database service.
        /// Returns true if reachable, otherwise false.
        /// </summary>
        Task<bool> PingAsync();
    }
}

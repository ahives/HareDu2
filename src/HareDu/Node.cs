namespace HareDu
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface Node :
        BrokerObject
    {
        /// <summary>
        /// Returns all nodes on the current RabbitMQ cluster.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<ResultList<NodeInfo>> GetAll(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes test to check if the current RabbitMQ node is healthy.
        /// </summary>
        /// <param name="node">Name of the RabbitMQ node.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result<NodeHealthInfo>> GetHealth(string node = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Returns the memory usage information on the specified RabbitMQ node.
        /// </summary>
        /// <param name="node">Name of the RabbitMQ node.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result<NodeMemoryUsageInfo>> GetMemoryUsage(string node, CancellationToken cancellationToken = default);
    }
}
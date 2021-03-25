namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface Server :
        BrokerObject
    {
        /// <summary>
        /// Returns all object definitions on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ServerInfo>> Get(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Perform a health check on a virtual host or node.
        /// </summary>
        /// <param name="action">Constraints of how the </param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<Result<ServerHealthInfo>> GetHealth(Action<HealthCheckAction> action, CancellationToken cancellationToken = default);
    }
}
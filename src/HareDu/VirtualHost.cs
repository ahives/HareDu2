namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface VirtualHost :
        BrokerObject
    {
        /// <summary>
        /// Returns information about each virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<ResultList<VirtualHostInfo>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action">Describes how the virtual host will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        [Obsolete("This method is deprecated, please use the overloaded method signature instead.")]
        Task<Result> Create(Action<VirtualHostCreateAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the specified virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action">Describes how the virtual host will be delete.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        [Obsolete("This method is deprecated, please use the overloaded method signature instead.")]
        Task<Result> Delete(Action<VirtualHostDeleteAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Starts up the specified RabbitMQ virtual host on the specified node.
        /// </summary>
        /// <param name="vhost"></param>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Obsolete("This method is deprecated, please use the overloaded method signature instead.")]
        Task<Result> Startup(string vhost, Action<VirtualHostStartupAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified RabbitMQ virtual host on the current server.
        /// </summary>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="configurator">Describes how the virtual host will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Create(string vhost, Action<VirtualHostConfigurator> configurator, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified RabbitMQ virtual host on the current server.
        /// </summary>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string vhost, CancellationToken cancellationToken = default);

        /// <summary>
        /// Starts up the specified RabbitMQ virtual host on the specified node.
        /// </summary>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="node">Name of the RabbitMQ server node.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Startup(string vhost, string node, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a health check on the specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result<ServerHealthInfo>> GetHealth(string vhost, CancellationToken cancellationToken = default);
    }
}
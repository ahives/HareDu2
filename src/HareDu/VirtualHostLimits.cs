namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface VirtualHostLimits :
        BrokerObject
    {
        /// <summary>
        /// Returns limit information about each virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<ResultList<VirtualHostLimitsInfo>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Defines specified limits on the virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Obsolete("This method is deprecated, please use the overloaded method signature instead.")]
        Task<Result> Define(Action<VirtualHostConfigureLimitsAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the limits for the specified virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Obsolete("This method is deprecated, please use the overloaded method signature instead.")]
        Task<Result> Delete(Action<VirtualHostDeleteLimitsAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Defines specified limits on the RabbitMQ virtual host on the current server.
        /// </summary>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="configurator">Describes how the virtual host limits will be defined.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Define(string vhost, Action<VirtualHostLimitsConfigurator> configurator = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the limits for the specified virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string vhost, CancellationToken cancellationToken = default);
    }
}
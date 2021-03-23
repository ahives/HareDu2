namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface Exchange :
        BrokerObject
    {
        /// <summary>
        /// Returns all exchanges on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<ResultList<ExchangeInfo>> GetAll(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates the specified exchange on the target virtual host. (deprecated)
        /// </summary>
        /// <param name="action">Describes how the queue will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Create(Action<ExchangeCreateAction> action, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete the specified exchange on the target virtual host. (deprecated)
        /// </summary>
        /// <param name="action">Describes how the queue will be deleted.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Delete(Action<ExchangeDeleteAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified exchange on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="exchange">Name of the RabbitMQ exchange.</param>
        /// <param name="vhost">Name of the RabbitMQ virtual host.</param>
        /// <param name="configurator">Describes how the queue will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Create(string exchange, string vhost, Action<ExchangeConfigurator> configurator = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified exchange on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="exchange">Name of the RabbitMQ exchange.</param>
        /// <param name="vhost">Name of the RabbitMQ virtual host.</param>
        /// <param name="configurator">Describes how the queue will be deleted.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string exchange, string vhost, Action<ExchangeDeletionConfigurator> configurator = null, CancellationToken cancellationToken = default);
    }
}
namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface Queue :
        BrokerObject
    {
        /// <summary>
        /// Returns all queues on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<ResultList<QueueInfo>> GetAll(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates the specified queue on the target virtual host and perhaps RabbitMQ node. (deprecated)
        /// </summary>
        /// <param name="action">Describes how the queue will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Create(Action<QueueCreateAction> action, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete the specified queue on the target virtual host and perhaps RabbitMQ node. (deprecated)
        /// </summary>
        /// <param name="action">Describes how the queue will be deleted.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Delete(Action<QueueDeleteAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Purge all messages in the specified queue on the target virtual host on the current RabbitMQ node. (deprecated)
        /// </summary>
        /// <param name="action">Describes how the queue will be purged.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Empty(Action<QueueEmptyAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates specified queue on the specified RabbitMQ virtual host and node.
        /// </summary>
        /// <param name="queue">Name of the RabbitMQ broker queue.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="node">Name of the RabbitMQ node.</param>
        /// <param name="configurator">Describes how the queue will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Create(string queue, string vhost, string node, Action<QueueConfigurator> configurator = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes specified queue on the specified RabbitMQ virtual host and node.
        /// </summary>
        /// <param name="queue">Name of the RabbitMQ broker queue.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="configurator">Describes how the queue should be deleted.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string queue, string vhost, Action<QueueDeletionConfigurator> configurator = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Purges all messages in the specified queue on the specified RabbitMQ virtual host on the current node.
        /// </summary>
        /// <param name="queue">Name of the RabbitMQ broker queue.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Empty(string queue, string vhost, CancellationToken cancellationToken = default);

        /// <summary>
        /// Syncs or cancels sync of specified RabbitMQ queue.
        /// </summary>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="syncAction">Sync action to be performed on RabbitMQ queue.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Sync(string queue, string vhost, QueueSyncAction syncAction, CancellationToken cancellationToken = default);
    }
}
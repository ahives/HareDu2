namespace HareDu.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class QueueExtensions
    {
        /// <summary>
        /// Returns all queues on the current RabbitMQ node.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<QueueInfo>> GetAllQueues(this IBrokerObjectFactory factory,
            CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Queue>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates specified queue on the specified RabbitMQ virtual host and node.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="queue">Name of the RabbitMQ broker queue.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="node">Name of the RabbitMQ node.</param>
        /// <param name="configurator">Describes how the queue will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CreateQueue(this IBrokerObjectFactory factory,
            string queue, string vhost, string node, Action<QueueConfigurator> configurator, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Queue>()
                .Create(queue, vhost, node, configurator, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Purges all messages in the specified queue on the specified RabbitMQ virtual host on the current node.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="queue">Name of the RabbitMQ broker queue.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> EmptyQueue(this IBrokerObjectFactory factory,
            string queue, string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Queue>()
                .Empty(queue, vhost, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes specified queue on the specified RabbitMQ virtual host and node.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="queue">Name of the RabbitMQ broker queue.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="configurator">Describes how the queue should be deleted.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeleteQueue(this IBrokerObjectFactory factory,
            string queue, string vhost, Action<QueueDeletionConfigurator> configurator = null, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Queue>()
                .Delete(queue, vhost, configurator, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Syncs of specified RabbitMQ queue.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="queue">Name of the RabbitMQ broker queue.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> SyncQueue(this IBrokerObjectFactory factory, string queue, string vhost,
            CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Queue>()
                .Sync(queue, vhost, QueueSyncAction.Sync, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels sync of specified RabbitMQ queue.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="queue">Name of the RabbitMQ broker queue.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CancelQueueSync(this IBrokerObjectFactory factory, string queue, string vhost,
            CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Queue>()
                .Sync(queue, vhost, QueueSyncAction.CancelSync, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
namespace HareDu.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class TopicPermissionsExtensions
    {
        /// <summary>
        /// Returns all the RabbitMQ topic permissions.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<TopicPermissionsInfo>> GetAllTopicPermissions(
            this IBrokerObjectFactory factory, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<TopicPermissions>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new topic permission for the specified user per a particular RabbitMQ exchange and virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="username">RabbitMQ broker username to apply topic permission to.</param>
        /// <param name="exchange">Name of the RabbitMQ exchange.</param>
        /// <param name="vhost">Name of the RabbitMQ virtual host.</param>
        /// <param name="configurator">Describes how the topic permission will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CreateTopicPermission(this IBrokerObjectFactory factory,
            string username, string exchange, string vhost, Action<TopicPermissionsConfigurator> configurator, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<TopicPermissions>()
                .Create(username, exchange, vhost, configurator, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes all topic permissions associate with the specified user on the specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="username">RabbitMQ broker username used to delete topic permission.</param>
        /// <param name="vhost">Name of the RabbitMQ virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeleteTopicPermission(this IBrokerObjectFactory factory,
            string username, string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<TopicPermissions>()
                .Delete(username, vhost, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
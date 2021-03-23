namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface TopicPermissions :
        BrokerObject
    {
        /// <summary>
        /// Returns all the RabbitMQ topic permissions.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<ResultList<TopicPermissionsInfo>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new topic permission for the specified user per a particular RabbitMQ exchange and virtual host. (deprecated)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> Create(Action<TopicPermissionsCreateAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes all topic permissions associate with the specified user on the specified RabbitMQ virtual host. (deprecated)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> Delete(Action<TopicPermissionsDeleteAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new topic permission for the specified user per a particular RabbitMQ exchange and virtual host.
        /// </summary>
        /// <param name="username">RabbitMQ broker username to apply topic permission to.</param>
        /// <param name="exchange">Name of the RabbitMQ exchange.</param>
        /// <param name="vhost">Name of the RabbitMQ virtual host.</param>
        /// <param name="configurator">Describes how the topic permission will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Create(string username, string exchange, string vhost, Action<TopicPermissionsConfigurator> configurator, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes all topic permissions associate with the specified user on the specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="username">RabbitMQ broker username used to delete topic permission.</param>
        /// <param name="vhost">Name of the RabbitMQ virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string username, string vhost, CancellationToken cancellationToken = default);
    }
}
namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface UserPermissions :
        BrokerObject
    {
        /// <summary>
        /// Returns information about all user permissions on the current RabbitMQ server.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<ResultList<UserPermissionsInfo>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a user permission and assign it to a user on a specific virtual host on the current RabbitMQ server. (deprecated)
        /// </summary>
        /// <param name="action">Describes how the user permission will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<Result> Create(Action<UserPermissionsCreateAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the specified user permission assigned to a specified user on a specific virtual host on the current RabbitMQ server. (deprecated)
        /// </summary>
        /// <param name="action">Describes how the virtual host will be delete.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<Result> Delete(Action<UserPermissionsDeleteAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a user permission and assign it to a user on a specific virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="username">RabbitMQ broker username.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="configurator">Describes how the user permissions will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Create(string username, string vhost, Action<UserPermissionsConfigurator> configurator, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified user permission assigned to a specified user on a specific virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="username">RabbitMQ broker username.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string username, string vhost, CancellationToken cancellationToken = default);
    }
}
namespace HareDu.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class UserPermissionsExtensions
    {
        /// <summary>
        /// Returns information about all user permissions on the current RabbitMQ server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<UserPermissionsInfo>> GetAllUserPermissions(this IBrokerObjectFactory factory,
            CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<UserPermissions>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a user permission and assign it to a user on a specific virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="username">RabbitMQ broker username.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="configurator">Describes how the user permissions will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CreateUserPermissions(this IBrokerObjectFactory factory,
            string username, string vhost, Action<UserPermissionsConfigurator> configurator, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            if (configurator.IsNull())
            {
                configurator = x =>
                {
                    x.UsingConfigurePattern(".*");
                    x.UsingReadPattern(".*");
                    x.UsingWritePattern(".*");
                };
            }
            
            return await factory.Object<UserPermissions>()
                .Create(username, vhost, configurator, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the specified user on the current RabbitMQ server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="username">RabbitMQ broker username.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeleteUserPermissions(this IBrokerObjectFactory factory, string username,
            string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<UserPermissions>()
                .Delete(username, vhost, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
namespace HareDu.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class VirtualHostExtensions
    {
        /// <summary>
        /// Returns information about each virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<VirtualHostInfo>> GetAllVirtualHosts(this IBrokerObjectFactory factory,
            CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<VirtualHost>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the specified RabbitMQ virtual host on the current server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="configurator">Describes how the virtual host will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CreateVirtualHost(this IBrokerObjectFactory factory,
            string vhost, Action<VirtualHostConfigurator> configurator, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<VirtualHost>()
                .Create(vhost, configurator, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the specified RabbitMQ virtual host on the current server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeleteVirtualHost(this IBrokerObjectFactory factory,
            string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<VirtualHost>()
                .Delete(vhost, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Starts up the specified RabbitMQ virtual host on the specified node.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="node">Name of the RabbitMQ server node.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> StartupVirtualHost(this IBrokerObjectFactory factory,
            string vhost, string node, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<VirtualHost>()
                .Startup(vhost, node, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Performs a health check on the specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result<ServerHealthInfo>> GetVirtualHostHealth(this IBrokerObjectFactory factory,
            string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<VirtualHost>()
                .GetHealth(vhost, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
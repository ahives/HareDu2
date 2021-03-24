namespace HareDu.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class VirtualHostLimitsExtensions
    {
        /// <summary>
        /// Returns limit information about each virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<VirtualHostLimitsInfo>> GetAllVirtualHostLimits(
            this IBrokerObjectFactory factory, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<VirtualHostLimits>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Defines specified limits on the RabbitMQ virtual host on the current server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="configurator">Describes how the virtual host limits will be defined.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DefineVirtualHostLimits(this IBrokerObjectFactory factory, string vhost,
            Action<VirtualHostLimitsConfigurator> configurator = null, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<VirtualHostLimits>()
                .Define(vhost, configurator, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the limits for the specified virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="vhost">Name of the RabbitMQ broker virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeleteVirtualHostLimits(this IBrokerObjectFactory factory, string vhost,
            CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<VirtualHostLimits>()
                .Delete(vhost, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
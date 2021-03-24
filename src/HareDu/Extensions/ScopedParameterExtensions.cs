namespace HareDu.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class ScopedParameterExtensions
    {
        /// <summary>
        /// Returns all scoped parameters on the current RabbitMQ server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<ScopedParameterInfo>> GetAllScopedParameters(this IBrokerObjectFactory factory,
            CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<ScopedParameter>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a scoped parameter for a particular RabbitMQ component and virtual host on the current server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="parameter">Name of the RabbitMQ parameter.</param>
        /// <param name="value">Value of the RabbitMQ parameter.</param>
        /// <param name="component">Name of the RabbitMQ component.</param>
        /// <param name="vhost">Name of the RabbitMQ virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CreateScopeParameter<T>(this IBrokerObjectFactory factory,
            string parameter, T value, string component, string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<ScopedParameter>()
                .Create(parameter, value, component, vhost, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the specified scoped parameter for a particular RabbitMQ component and virtual host on the current server.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="parameter">Name of the RabbitMQ parameter.</param>
        /// <param name="component">Name of the RabbitMQ component.</param>
        /// <param name="vhost">Name of the RabbitMQ virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeleteScopedParameter(this IBrokerObjectFactory factory,
            string parameter, string component, string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<ScopedParameter>()
                .Delete(parameter, component, vhost, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
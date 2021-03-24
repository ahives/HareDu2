namespace HareDu.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class ExchangeExtensions
    {
        /// <summary>
        /// Returns all exchanges on the current RabbitMQ node.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<ExchangeInfo>> GetAllExchanges(this IBrokerObjectFactory factory,
            CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Exchange>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the specified exchange on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="exchange">Name of the RabbitMQ exchange.</param>
        /// <param name="vhost">Name of the RabbitMQ virtual host.</param>
        /// <param name="configurator">Describes how the queue will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CreateExchange(this IBrokerObjectFactory factory,
            string exchange, string vhost, Action<ExchangeConfigurator> configurator = null, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Exchange>()
                .Create(exchange, vhost, configurator, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the specified exchange on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="exchange">Name of the RabbitMQ exchange.</param>
        /// <param name="vhost">Name of the RabbitMQ virtual host.</param>
        /// <param name="configurator">Describes how the queue will be deleted.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeleteExchange(this IBrokerObjectFactory factory,
            string exchange, string vhost, Action<ExchangeDeletionConfigurator> configurator = null, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Exchange>()
                .Delete(exchange, vhost, configurator, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
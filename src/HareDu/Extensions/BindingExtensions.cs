namespace HareDu.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class BindingExtensions
    {
        /// <summary>
        /// Returns all bindings on the current RabbitMQ node.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<BindingInfo>> GetAllBindings(this IBrokerObjectFactory factory, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Binding>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the specified binding between source queue and destination queue on the specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="sourceBinding">Source binding of the exchange.</param>
        /// <param name="destinationBinding">Destination binding of the exchange.</param>
        /// <param name="vhost">The virtual host where the binding is defined.</param>
        /// <param name="bindingKey">The routing pattern for a source to destination binding.</param>
        /// <param name="configurator">Describes how the binding will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CreateExchangeBindingToQueue(this IBrokerObjectFactory factory,
            string sourceBinding, string destinationBinding, string vhost, string bindingKey = null, Action<BindingConfigurator> configurator = null, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Binding>()
                .Create(sourceBinding, destinationBinding, BindingType.Queue, vhost, bindingKey, configurator, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the specified binding between source exchange and destination exchange on the specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="sourceBinding">Source binding of the exchange.</param>
        /// <param name="destinationBinding">Destination binding of the exchange.</param>
        /// <param name="vhost">The virtual host where the binding is defined.</param>
        /// <param name="bindingKey">The routing pattern for a source to destination binding.</param>
        /// <param name="configurator">Describes how the binding will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CreateExchangeBinding(this IBrokerObjectFactory factory,
            string sourceBinding, string destinationBinding, string vhost, string bindingKey = null, Action<BindingConfigurator> configurator = null, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Binding>()
                .Create(sourceBinding, destinationBinding, BindingType.Exchange, vhost, bindingKey, configurator, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the specified binding between exchange and queue on the specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="sourceBinding">Source binding of the queue.</param>
        /// <param name="destinationBinding">Destination binding of the queue.</param>
        /// <param name="propertiesKey">Combination of routing key and hash of its arguments.</param>
        /// <param name="vhost">The virtual host where the binding is defined.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeleteQueueBinding(this IBrokerObjectFactory factory,
            string sourceBinding, string destinationBinding, string propertiesKey, string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Binding>()
                .Delete(sourceBinding, destinationBinding, propertiesKey, vhost, BindingType.Queue, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the specified binding between exchanges on the specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="sourceBinding">Source binding of the exchange.</param>
        /// <param name="destinationBinding">Destination binding of the exchange.</param>
        /// <param name="propertiesKey">Combination of routing key and hash of its arguments.</param>
        /// <param name="vhost">The virtual host where the binding is defined.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeleteExchangeBinding(this IBrokerObjectFactory factory,
            string sourceBinding, string destinationBinding, string propertiesKey, string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Binding>()
                .Delete(sourceBinding, destinationBinding, propertiesKey, vhost, BindingType.Exchange, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
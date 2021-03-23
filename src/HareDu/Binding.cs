namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface Binding :
        BrokerObject
    {
        /// <summary>
        /// Returns all bindings on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<ResultList<BindingInfo>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified binding between source (i.e. queue/exchange) and target (i.e. queue/exchange) on the target virtual host. (deprecated)
        /// </summary>
        /// <param name="action">Describes how the queue will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<BindingInfo>> Create(Action<BindingCreateAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the specified exchange on the target virtual host. (deprecated)
        /// </summary>
        /// <param name="action">Describes how the queue will be deleted.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Delete(Action<BindingDeleteAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified binding between source (i.e. queue/exchange) and destination (i.e. queue/exchange) on the specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="sourceBinding">Source binding of the exchange/queue depending on <see cref="bindingType"/>.</param>
        /// <param name="destinationBinding">Destination binding of the exchange/queue depending on <see cref="bindingType"/>.</param>
        /// <param name="bindingType">The type of binding, exchange or queue.</param>
        /// <param name="vhost">The virtual host where the binding is defined.</param>
        /// <param name="bindingKey">The routing pattern for a source to destination binding.</param>
        /// <param name="configurator">Describes how the binding will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result<BindingInfo>> Create(string sourceBinding, string destinationBinding, BindingType bindingType, string vhost,
            string bindingKey = null, Action<BindingConfigurator> configurator = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified exchange on the specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="sourceBinding">Source binding of the exchange/queue depending on <see cref="bindingType"/>.</param>
        /// <param name="destinationBinding">Destination binding of the exchange/queue depending on <see cref="bindingType"/>.</param>
        /// <param name="propertiesKey">Combination of routing key and hash of its arguments.</param>
        /// <param name="vhost">The virtual host where the binding is defined.</param>
        /// <param name="bindingType">The type of binding, exchange or queue.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string sourceBinding, string destinationBinding, string propertiesKey,
            string vhost, BindingType bindingType, CancellationToken cancellationToken = default);
    }
}
namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface GlobalParameter :
        BrokerObject
    {
        /// <summary>
        /// Returns all global parameters on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<ResultList<GlobalParameterInfo>> GetAll(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates the specified global parameter on the current RabbitMQ node. (deprecated)
        /// </summary>
        /// <param name="action">Describes how the global parameter will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Create(Action<GlobalParameterCreateAction> action, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete the specified global parameter on the current RabbitMQ node. (deprecated)
        /// </summary>
        /// <param name="action">Describes how the global parameter will be deleted.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Delete(Action<GlobalParameterDeleteAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified global parameter on the current RabbitMQ node.
        /// </summary>
        /// <param name="parameter">Name of the RabbitMQ parameter.</param>
        /// <param name="configurator">Describes how the RabbitMQ parameter is to be defined.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Create(string parameter, Action<GlobalParameterConfigurator> configurator, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified global parameter on the current RabbitMQ node.
        /// </summary>
        /// <param name="parameter">Name of the RabbitMQ parameter.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string parameter, CancellationToken cancellationToken = default);
    }

    public interface GlobalParameterConfigurator
    {
        /// <summary>
        /// Specify global parameter arguments.
        /// </summary>
        /// <param name="configurator"></param>
        void Value(Action<GlobalParameterArgumentConfigurator> configurator);
        
        /// <summary>
        /// Specify global parameter argument.
        /// </summary>
        /// <param name="arguments"></param>
        void Value<T>(T argument);
    }

    public interface GlobalParameterArgumentConfigurator
    {
        /// <summary>
        /// Create a new argument.
        /// </summary>
        /// <param name="arg">Name of the argument.</param>
        /// <param name="value">Value of the argument.</param>
        /// <typeparam name="T"></typeparam>
        void Add<T>(string arg, T value);
    }
}
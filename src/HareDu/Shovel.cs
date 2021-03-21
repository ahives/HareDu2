namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface Shovel :
        BrokerObject
    {
        /// <summary>
        /// Returns all dynamic shovels that have been created.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<ResultList<ShovelInfo>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a dynamic shovel on a specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="shovel">The name of the dynamic shovel.</param>
        /// <param name="uri">The connection URI of the RabbitMQ broker.</param>
        /// <param name="vhost">The virtual host where the shovel resides.</param>
        /// <param name="configurator">Describes how the dynamic shovel will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Create(string shovel, string uri, string vhost, Action<ShovelConfigurator> configurator, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Deletes a dynamic shovel on a specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="shovel">The name of the dynamic shovel.</param>
        /// <param name="vhost">The virtual host where the dynamic shovel resides.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string shovel, string vhost, CancellationToken cancellationToken = default);
    }
}
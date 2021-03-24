namespace HareDu.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class ShovelExtensions
    {
        /// <summary>
        /// Creates a dynamic shovel on a specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="shovel">The name of the dynamic shovel.</param>
        /// <param name="vhost">The virtual host where the shovel resides.</param>
        /// <param name="configurator">Describes how the dynamic shovel will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CreateShovel(this IBrokerObjectFactory factory,
            string shovel, string uri, string vhost, Action<ShovelConfigurator> configurator = null, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Shovel>()
                .Create(shovel, uri, vhost, configurator, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a dynamic shovel on a specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="shovel">The name of the dynamic shovel.</param>
        /// <param name="vhost">The virtual host where the dynamic shovel resides.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeleteShovel(this IBrokerObjectFactory factory,
            string shovel, string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));
            
            return await factory.Object<Shovel>()
                .Delete(shovel, vhost, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes all dynamic shovels on a specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="vhost">The virtual host where the dynamic shovel resides.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<IReadOnlyList<Result>> DeleteAllShovels(this IBrokerObjectFactory factory,
            string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));
            
            var result = await factory.Object<Shovel>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);

            if (result.HasFaulted)
                return new List<Result>();
            
            var shovels = result
                .Select(x => x.Data)
                .Where(x => x.VirtualHost == vhost && x.Type == ShovelType.Dynamic)
                .ToList();

            var results = new List<Result>();
            
            foreach (var shovel in shovels)
            {
                var deleteResult = await factory.Object<Shovel>()
                    .Delete(shovel.Name, vhost, cancellationToken)
                    .ConfigureAwait(false);
                
                results.Add(deleteResult);
            }

            return results;
        }
        
        /// <summary>
        /// Returns all dynamic shovels that have been created.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<ShovelInfo>> GetAllShovels(this IBrokerObjectFactory factory, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));
            
            return await factory.Object<Shovel>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
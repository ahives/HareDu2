namespace HareDu.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class NodeExtensions
    {
        /// <summary>
        /// Returns all nodes on the current RabbitMQ cluster.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<NodeInfo>> GetAllNodes(this IBrokerObjectFactory factory,
            CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Node>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Executes test to check if the current RabbitMQ node is healthy.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="node">Name of the RabbitMQ node.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result<NodeHealthInfo>> GetNodeHealth(this IBrokerObjectFactory factory,
            string node = null, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Node>()
                .GetHealth(node, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the memory usage information on the specified RabbitMQ node.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="node">Name of the RabbitMQ node.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result<NodeMemoryUsageInfo>> GetNodeMemoryUsage(this IBrokerObjectFactory factory,
            string node, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Node>()
                .GetMemoryUsage(node, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
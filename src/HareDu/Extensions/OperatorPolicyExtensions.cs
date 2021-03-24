namespace HareDu.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class OperatorPolicyExtensions
    {
        /// <summary>
        /// Returns all operator policies on the current RabbitMQ node.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<OperatorPolicyInfo>> GetAllOperatorPolicies(this IBrokerObjectFactory factory,
            CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<OperatorPolicy>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the specified operator policy on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="policy">Name of the operator policy.</param>
        /// <param name="pattern">The pattern to apply the policy on.</param>
        /// <param name="vhost">The virtual host for which the policy should be applied to.</param>
        /// <param name="configurator">Describes how the operator policy will be created by setting arguments through set methods.</param>
        /// <param name="appliedTo">The broker object for which the policy is to be applied to.</param>
        /// <param name="priority"></param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CreateOperatorPolicy(this IBrokerObjectFactory factory, string policy,
            string pattern, string vhost, Action<OperatorPolicyConfigurator> configurator,
            OperatorPolicyAppliedTo appliedTo = default, int priority = default, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<OperatorPolicy>()
                .Create(policy, pattern, vhost, configurator, appliedTo, priority, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the specified operator policy on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="policy">Name of the operator policy.</param>
        /// <param name="vhost">The virtual host for which the policy should be applied to.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeleteOperatorPolicy(this IBrokerObjectFactory factory,
            string policy, string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<OperatorPolicy>()
                .Delete(policy, vhost, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
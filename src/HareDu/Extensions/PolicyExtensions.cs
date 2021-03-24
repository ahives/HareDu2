namespace HareDu.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class PolicyExtensions
    {
        /// <summary>
        /// Returns all policies on the current RabbitMQ node.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<ResultList<PolicyInfo>> GetAllPolicies(this IBrokerObjectFactory factory,
            CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Policy>()
                .GetAll(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the specified policy on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="policy">The name of the policy.</param>
        /// <param name="pattern">The pattern for which the policy is to be applied.</param>
        /// <param name="vhost">The name of the virtual host.</param>
        /// <param name="configurator">Describes how the policy will be created by setting arguments through set methods.</param>
        /// <param name="appliedTo">The type of broker objects to apply the policy to.</param>
        /// <param name="priority"></param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> CreatePolicy(this IBrokerObjectFactory factory,
            string policy, string pattern, string vhost, Action<PolicyConfigurator> configurator,
            PolicyAppliedTo appliedTo = PolicyAppliedTo.All, int priority = 0, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Policy>()
                .Create(policy, pattern, vhost, configurator, appliedTo, priority, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the specified policy on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="factory">The object factory that implements the underlying functionality.</param>
        /// <param name="policy">The name of the policy.</param>
        /// <param name="vhost">The name of the virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if BrokerObjectFactory is null.</exception>
        public static async Task<Result> DeletePolicy(this IBrokerObjectFactory factory,
            string policy, string vhost, CancellationToken cancellationToken = default)
        {
            if (factory.IsNull())
                throw new ArgumentNullException(nameof(factory));

            return await factory.Object<Policy>()
                .Delete(policy, vhost, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
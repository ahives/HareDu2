namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface OperatorPolicy :
        BrokerObject
    {
        /// <summary>
        /// Returns all operator policies on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns>A <see cref="ResultList{TResult}"/> of operator policies.</returns>
        Task<ResultList<OperatorPolicyInfo>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified operator policy on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="policy">Name of the operator policy.</param>
        /// <param name="pattern">The pattern to apply the policy on.</param>
        /// <param name="vhost">The virtual host for which the policy should be applied to.</param>
        /// <param name="configurator">Describes how the operator policy will be created by setting arguments through set methods.</param>
        /// <param name="appliedTo">The broker object for which the policy is to be applied to.</param>
        /// <param name="priority"></param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Create(string policy, string pattern, string vhost, Action<OperatorPolicyConfigurator> configurator,
            OperatorPolicyAppliedTo appliedTo = default, int priority = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified operator policy on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="policy">Name of the operator policy.</param>
        /// <param name="vhost">The virtual host for which the policy should be applied to.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string policy, string vhost, CancellationToken cancellationToken = default);
    }
}
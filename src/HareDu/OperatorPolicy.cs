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

    public enum OperatorPolicyAppliedTo
    {
        Queues
    }

    public interface OperatorPolicyConfigurator
    {
        /// <summary>
        /// Set 'message-ttl' argument on the operator policy.
        /// </summary>
        /// <param name="milliseconds"></param>
        void SetMessageTimeToLive(ulong milliseconds);

        /// <summary>
        /// Set 'max-length-bytes' argument on the operator policy.
        /// </summary>
        /// <param name="value"></param>
        void SetMessageMaxSizeInBytes(ulong value);

        /// <summary>
        /// Set 'max-length' argument on the operator policy.
        /// </summary>
        /// <param name="value"></param>
        void SetMessageMaxSize(ulong value);

        /// <summary>
        /// Set 'expires' argument on the operator policy.
        /// </summary>
        /// <param name="milliseconds"></param>
        void SetExpiry(ulong milliseconds);

        /// <summary>
        /// Set 'x-max-in-memory-bytes' argument on the operator policy.
        /// </summary>
        /// <param name="bytes"></param>
        void SetMaxInMemoryBytes(ulong bytes);
        
        /// <summary>
        /// Set 'x-max-in-memory-length' argument on the operator policy.
        /// </summary>
        /// <param name="???"></param>
        void SetMaxInMemoryLength(ulong messages);
        
        /// <summary>
        /// Set 'delivery-limit' argument on the operator policy.
        /// </summary>
        void SetDeliveryLimit(ulong limit);
    }
}
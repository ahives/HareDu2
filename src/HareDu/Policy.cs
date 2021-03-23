namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface Policy :
        BrokerObject
    {
        /// <summary>
        /// Returns all policies on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<ResultList<PolicyInfo>> GetAll(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates the specified policy on the target virtual host.
        /// </summary>
        /// <param name="action">Describes how the policy will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Create(Action<PolicyCreateAction> action, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete the specified policy on the target virtual host.
        /// </summary>
        /// <param name="action">Describes how the policy will be deleted.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Delete(Action<PolicyDeleteAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified policy on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="policy">The name of the policy.</param>
        /// <param name="pattern">The pattern for which the policy is to be applied.</param>
        /// <param name="vhost">The name of the virtual host.</param>
        /// <param name="configurator">Describes how the policy will be created by setting arguments through set methods.</param>
        /// <param name="appliedTo">The type of broker objects to apply the policy to.</param>
        /// <param name="priority"></param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Create(string policy, string pattern, string vhost, Action<PolicyConfigurator> configurator,
            PolicyAppliedTo appliedTo = PolicyAppliedTo.All, int priority = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified policy on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="policy">The name of the policy.</param>
        /// <param name="vhost">The name of the virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string policy, string vhost, CancellationToken cancellationToken = default);
    }

    public enum PolicyAppliedTo
    {
        All,
        Exchanges,
        Queues
    }

    public interface PolicyConfigurator
    {
        /// <summary>
        /// Set user-defined argument on the policy.
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        void Set<T>(string arg, T value);

        /// <summary>
        /// Set 'expires' argument on the policy.
        /// </summary>
        /// <param name="milliseconds"></param>
        void SetExpiry(ulong milliseconds);

        /// <summary>
        /// Set 'federation-upstream-set' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetFederationUpstreamSet(string value);

        /// <summary>
        /// Set 'federation-upstream' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetFederationUpstream(string value);

        /// <summary>
        /// Set 'ha-mode' argument on the policy.
        /// </summary>
        /// <param name="mode"></param>
        void SetHighAvailabilityMode(HighAvailabilityModes mode);

        /// <summary>
        /// Set 'ha-params' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetHighAvailabilityParams(uint value);

        /// <summary>
        /// Set 'ha-sync-mode' argument on the policy.
        /// </summary>
        /// <param name="mode"></param>
        void SetHighAvailabilitySyncMode(HighAvailabilitySyncMode mode);

        /// <summary>
        /// Set 'message-ttl' argument on the policy.
        /// </summary>
        /// <param name="milliseconds"></param>
        void SetMessageTimeToLive(ulong milliseconds);

        /// <summary>
        /// Set 'max-length-bytes' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetMessageMaxSizeInBytes(ulong value);

        /// <summary>
        /// Set 'max-length' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetMessageMaxSize(ulong value);

        /// <summary>
        /// Set 'dead-letter-exchange' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetDeadLetterExchange(string value);

        /// <summary>
        /// Set 'dead-letter-routing-key' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetDeadLetterRoutingKey(string value);

        /// <summary>
        /// Set 'queue-mode' argument on the policy.
        /// </summary>
        void SetQueueMode(QueueMode mode);

        /// <summary>
        /// Set 'alternate-exchange' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetAlternateExchange(string value);

        /// <summary>
        /// Set 'queue-master-locator' argument on the policy.
        /// </summary>
        /// <param name="key"></param>
        void SetQueueMasterLocator(string key);

        /// <summary>
        /// Set 'ha-promote-on-shutdown' argument on the policy.
        /// </summary>
        void SetQueuePromotionOnShutdown(QueuePromotionShutdownMode mode);

        /// <summary>
        /// Set 'ha-promote-on-failure' argument on the policy.
        /// </summary>
        void SetQueuePromotionOnFailure(QueuePromotionFailureMode mode);
        
        /// <summary>
        /// Set 'delivery-limit' argument on the operator policy.
        /// </summary>
        void SetDeliveryLimit(ulong limit);
    }

    public enum QueueMode
    {
        Default,
        Lazy
    }

    public enum HighAvailabilitySyncMode
    {
        Manual,
        Automatic
    }

    public enum QueuePromotionFailureMode
    {
        Always,
        WhenSynced
    }

    public enum QueuePromotionShutdownMode
    {
        Always,
        WhenSynced
    }
}
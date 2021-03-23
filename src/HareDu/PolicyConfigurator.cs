namespace HareDu
{
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
}
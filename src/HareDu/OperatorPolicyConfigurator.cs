namespace HareDu
{
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
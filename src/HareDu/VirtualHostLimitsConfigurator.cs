namespace HareDu
{
    public interface VirtualHostLimitsConfigurator
    {
        /// <summary>
        /// Set the 'max-connections' RabbitMQ virtual host limit value.
        /// </summary>
        /// <param name="value"></param>
        void SetMaxConnectionLimit(ulong value);

        /// <summary>
        /// Set the 'max-queues' RabbitMQ virtual host limit value.
        /// </summary>
        /// <param name="value"></param>
        void SetMaxQueueLimit(ulong value);
    }
}
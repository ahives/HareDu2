namespace HareDu
{
    public interface ShovelSourceConfigurator
    {
        /// <summary>
        /// Sets the protocol that is to be used to shovel messages.
        /// </summary>
        /// <param name="protocol">Type of protocol to use to define the source end of the shovel.</param>
        void Protocol(ShovelProtocolType protocol);

        /// <summary>
        /// Determines when the shovel should delete itself.
        /// </summary>
        /// <param name="mode">Determines when the shovel should delete itself.</param>
        void DeleteAfter(DeleteShovelMode mode);
        
        /// <summary>
        /// The shovel will delete itself after the number of <see cref="messages"/> is reached.
        /// </summary>
        /// <param name="messages">The number of messages to transfer before the shovel deletes itself.</param>
        void DeleteAfter(uint messages);

        /// <summary>
        /// Sets the prefetch count on the shovel.
        /// </summary>
        /// <param name="messages">Maximum number of messages to be copied per shovel run.</param>
        void MaxCopiedMessages(ulong messages);

        /// <summary>
        /// Sets the exchange and its routing key.
        /// </summary>
        /// <param name="exchange">Name of the RabbitMQ exchange.</param>
        /// <param name="routingKey">Pattern defining how messages are to be routed to and from exchanges/queues.</param>
        void Exchange(string exchange, string routingKey = null);
    }
}
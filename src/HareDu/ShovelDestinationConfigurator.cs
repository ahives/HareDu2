namespace HareDu
{
    public interface ShovelDestinationConfigurator
    {
        /// <summary>
        /// Sets the protocol that is to be used to shovel messages.
        /// </summary>
        /// <param name="protocol">Type of protocol to use to define the destination end of the shovel.</param>
        void Protocol(ShovelProtocolType protocol);

        /// <summary>
        /// Sets the RabbitMQ exchange and its routing key.
        /// </summary>
        /// <param name="exchange">Name of the RabbitMQ exchange.</param>
        /// <param name="routingKey">Pattern defining how messages are to be routed to and from exchanges/queues.</param>
        void Exchange(string exchange, string routingKey = null);

        /// <summary>
        /// Sets the 'x-shovelled' header on messages that are shoveled.
        /// </summary>
        void AddForwardHeaders();
        
        /// <summary>
        /// Sets the 'x-shovelled-timestamp' header on shoveled messages.
        /// </summary>
        void AddTimestampHeaderToMessage();
    }
}
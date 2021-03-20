namespace HareDu
{
    using System;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface Shovel :
        BrokerObject
    {
        /// <summary>
        /// Returns all dynamic shovels that have been created.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<ResultList<ShovelInfo>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a dynamic shovel on a specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="shovel">The name of the dynamic shovel.</param>
        /// <param name="uri">The connection URI of the RabbitMQ broker.</param>
        /// <param name="vhost">The virtual host where the shovel resides.</param>
        /// <param name="configurator">Describes how the dynamic shovel will be created.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Create(string shovel, string uri, string vhost, Action<ShovelConfigurator> configurator, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Deletes a dynamic shovel on a specified RabbitMQ virtual host.
        /// </summary>
        /// <param name="shovel">The name of the dynamic shovel.</param>
        /// <param name="vhost">The virtual host where the dynamic shovel resides.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string shovel, string vhost, CancellationToken cancellationToken = default);
    }

    public interface ShovelConfigurator
    {
        /// <summary>
        /// The duration to wait before reconnecting to the brokers after being disconnected at either end.
        /// </summary>
        /// <param name="delayInSeconds"></param>
        void ReconnectDelay(int delayInSeconds);

        /// <summary>
        /// Determine how the shovel should acknowledge consumed messages.
        /// </summary>
        /// <param name="mode">Define how the shovel will acknowledge consumed messages.</param>
        void AcknowledgementMode(AckMode mode);
        
        /// <summary>
        /// Describes how the shovel is confirmed from the source end.
        /// </summary>
        /// <param name="queue">The name of the source queue.</param>
        /// <param name="configurator">Describes the source shovel.</param>
        void Source(string queue, Action<ShovelSourceConfigurator> configurator = null);
        
        /// <summary>
        /// Describes how the shovel is confirmed from the destination end.
        /// </summary>
        /// <param name="queue">The name of the destination queue.</param>
        /// <param name="configurator">Describes the destination shovel.</param>
        void Destination(string queue, Action<ShovelDestinationConfigurator> configurator = null);
    }

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

    public enum DeleteShovelMode
    {
        Never,
        QueueLength
    }

    public enum ShovelProtocolType
    {
        Amqp091,
        Amqp10
    }

    public enum AckMode
    {
        [EnumMember(Value = "on-confirm")]
        OnConfirm,
        
        [EnumMember(Value = "on-publish")]
        OnPublish,
        
        [EnumMember(Value = "no-ack")]
        NoAck
    }
}
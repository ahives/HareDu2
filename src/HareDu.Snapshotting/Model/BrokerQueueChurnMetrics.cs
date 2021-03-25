namespace HareDu.Snapshotting.Model
{
    public interface BrokerQueueChurnMetrics
    {
        /// <summary>
        /// Total number of messages persisted to Mnesia.
        /// </summary>
        long Persisted { get; }
        
        /// <summary>
        /// Total number of messages published to the queue and rate at which they were sent and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Incoming { get; }
        
        /// <summary>
        /// Total number of messages sitting in the queue that were consumed but not acknowledged and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Unacknowledged { get; }
        
        /// <summary>
        /// Total number of messages sitting in the queue ready to be delivered to consumers and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Ready { get; }
        
        /// <summary>
        /// Total number of "Get" operations on the queue with acknowledgement and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Gets { get; }
        
        /// <summary>
        /// Total number of "Get" operations on the queue without acknowledgement and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth GetsWithoutAck { get; }
        
        /// <summary>
        /// Total number of messages that were delivered to consumers and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Delivered { get; }
        
        /// <summary>
        /// Total number of messages that were delivered to consumers without acknowledgement and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth DeliveredWithoutAck { get; }
        
        /// <summary>
        /// Total number of messages delivered to consumers via "Get" operations and the corresponding rate in msg/s. 
        /// </summary>
        QueueDepth DeliveredGets { get; }
        
        /// <summary>
        /// Total number/rate (msg/s) of messages that were redelivered to consumers.
        /// </summary>
        QueueDepth Redelivered { get; }
        
        /// <summary>
        /// Total number of messages that were acknowledged to have been consumed and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth Acknowledged { get; }
        
        /// <summary>
        /// Total number of messages that could not be routed to a queue and the corresponding rate in msg/s.
        /// </summary>
        QueueDepth NotRouted { get; }
        
        /// <summary>
        /// Total number of messages that 
        /// </summary>
        QueueDepth Broker { get; }
    }
}
namespace HareDu.Snapshotting.Model
{
    using System;

    public interface QueueSnapshot
    {
        /// <summary>
        /// Name of the queue.
        /// </summary>
        string Identifier { get; }
        
        /// <summary>
        /// Name of the virtual host that the queue belongs to.
        /// </summary>
        string VirtualHost { get; }
        
        /// <summary>
        /// Name of the physical node the queue is on.
        /// </summary>
        string Node { get; }
        
        /// <summary>
        /// 
        /// </summary>
        QueueChurnMetrics Messages { get; }
        
        /// <summary>
        /// 
        /// </summary>
        QueueMemoryDetails Memory { get; }
        
        QueueInternals Internals { get; }
        
        ulong Consumers { get; }
        
        decimal ConsumerUtilization { get; }
        
        DateTimeOffset IdleSince { get; }
    }
}
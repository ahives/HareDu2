namespace HareDu.Model
{
    using System;
    using System.Collections.Generic;

    public interface QueueInfo
    {
        Rate MessageDetails { get; }
        
        ulong TotalMessages { get; }
        
        Rate UnacknowledgedMessageDetails { get; }
        
        ulong UnacknowledgedMessages { get; }
        
        Rate ReadyMessageDetails { get; }
        
        ulong ReadyMessages { get; }
        
        Rate ReductionDetails { get; }
        
        long TotalReductions { get; }
        
        IDictionary<string, object> Arguments { get; }
        
        bool Exclusive { get; }
        
        bool AutoDelete { get; }
        
        bool Durable { get; }
        
        string VirtualHost { get; }
        
        string Name { get; }
        
        string Node { get; }
        
        ulong MessageBytesPagedOut { get; }
        
        ulong TotalMessagesPagedOut { get; }
        
        BackingQueueStatus BackingQueueStatus { get; }
        
        DateTimeOffset HeadMessageTimestamp { get; }
        
        ulong MessageBytesPersisted { get; }
        
        ulong MessageBytesInRAM { get; }
        
        ulong TotalBytesOfMessagesDeliveredButUnacknowledged { get; }
        
        ulong TotalBytesOfMessagesReadyForDelivery { get; }
        
        ulong TotalBytesOfAllMessages { get; }
        
        ulong MessagesPersisted { get; }
        
        ulong UnacknowledgedMessagesInRAM { get; }
        
        ulong MessagesReadyForDeliveryInRAM { get; }
        
        ulong MessagesInRAM { get; }
        
        GarbageCollectionDetails GC { get; }
        
        QueueState State { get; }
        
        IList<string> RecoverableSlaves { get; }
        
        ulong Consumers { get; }
        
        string ExclusiveConsumerTag { get; }
        
        string Policy { get; }
        
        decimal ConsumerUtilization { get; }
        
        DateTimeOffset IdleSince { get; }
        
        ulong Memory { get; }
        
        QueueMessageStats MessageStats { get; }
    }
}
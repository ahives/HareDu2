namespace HareDu.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface QueueInfo
    {
        [JsonPropertyName("messages_details")]
        Rate MessageDetails { get; }
        
        [JsonPropertyName("messages")]
        ulong TotalMessages { get; }
        
        [JsonPropertyName("messages_unacknowledged_details")]
        Rate UnacknowledgedMessageDetails { get; }
        
        [JsonPropertyName("messages_unacknowledged")]
        ulong UnacknowledgedMessages { get; }
        
        [JsonPropertyName("messages_ready_details")]
        Rate ReadyMessageDetails { get; }
        
        [JsonPropertyName("messages_ready")]
        ulong ReadyMessages { get; }
        
        [JsonPropertyName("reductions_details")]
        Rate ReductionDetails { get; }
        
        [JsonPropertyName("reductions")]
        long TotalReductions { get; }
        
        [JsonPropertyName("arguments")]
        IDictionary<string, object> Arguments { get; }
        
        [JsonPropertyName("exclusive")]
        bool Exclusive { get; }
        
        [JsonPropertyName("auto_delete")]
        bool AutoDelete { get; }
        
        [JsonPropertyName("durable")]
        bool Durable { get; }
        
        [JsonPropertyName("vhost")]
        string VirtualHost { get; }
        
        [JsonPropertyName("name")]
        string Name { get; }
        
        [JsonPropertyName("node")]
        string Node { get; }
        
        [JsonPropertyName("message_bytes_paged_out")]
        ulong MessageBytesPagedOut { get; }
        
        [JsonPropertyName("messages_paged_out")]
        ulong TotalMessagesPagedOut { get; }
        
        [JsonPropertyName("backing_queue_status")]
        BackingQueueStatus BackingQueueStatus { get; }
        
        [JsonPropertyName("head_message_timestamp")]
        DateTimeOffset HeadMessageTimestamp { get; }
        
        [JsonPropertyName("message_bytes_persistent")]
        ulong MessageBytesPersisted { get; }
        
        [JsonPropertyName("message_bytes_ram")]
        ulong MessageBytesInRAM { get; }
        
        [JsonPropertyName("message_bytes_unacknowledged")]
        ulong TotalBytesOfMessagesDeliveredButUnacknowledged { get; }
        
        [JsonPropertyName("message_bytes_ready")]
        ulong TotalBytesOfMessagesReadyForDelivery { get; }
        
        [JsonPropertyName("message_bytes")]
        ulong TotalBytesOfAllMessages { get; }
        
        [JsonPropertyName("messages_persistent")]
        ulong MessagesPersisted { get; }
        
        [JsonPropertyName("messages_unacknowledged_ram")]
        ulong UnacknowledgedMessagesInRAM { get; }
        
        [JsonPropertyName("messages_ready_ram")]
        ulong MessagesReadyForDeliveryInRAM { get; }
        
        [JsonPropertyName("messages_ram")]
        ulong MessagesInRAM { get; }
        
        [JsonPropertyName("garbage_collection")]
        GarbageCollectionDetails GC { get; }
        
        [JsonPropertyName("state")]
        string State { get; }
        
        [JsonPropertyName("recoverable_slaves")]
        IList<string> RecoverableSlaves { get; }
        
        [JsonPropertyName("consumers")]
        ulong Consumers { get; }
        
        [JsonPropertyName("exclusive_consumer_tag")]
        string ExclusiveConsumerTag { get; }
        
        [JsonPropertyName("policy")]
        string Policy { get; }
        
        [JsonPropertyName("consumer_utilisation")]
        decimal ConsumerUtilization { get; }
        
        [JsonPropertyName("idle_since")]
        DateTimeOffset IdleSince { get; }
        
        [JsonPropertyName("memory")]
        ulong Memory { get; }
        
        [JsonPropertyName("message_stats")]
        QueueMessageStats MessageStats { get; }
    }
}
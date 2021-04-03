namespace HareDu.Internal.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using HareDu.Model;

    class QueueInfoImpl
    {
        [JsonPropertyName("messages_details")]
        public RateImpl MessageDetails { get; set; }
        
        [JsonPropertyName("messages")]
        public ulong TotalMessages { get; set; }
        
        [JsonPropertyName("messages_unacknowledged_details")]
        public RateImpl UnacknowledgedMessageDetails { get; set; }
        
        [JsonPropertyName("messages_unacknowledged")]
        public ulong UnacknowledgedMessages { get; set; }
        
        [JsonPropertyName("messages_ready_details")]
        public RateImpl ReadyMessageDetails { get; set; }
        
        [JsonPropertyName("messages_ready")]
        public ulong ReadyMessages { get; set; }
        
        [JsonPropertyName("reductions_details")]
        public RateImpl ReductionDetails { get; set; }
        
        [JsonPropertyName("reductions")]
        public long TotalReductions { get; set; }
        
        [JsonPropertyName("arguments")]
        public IDictionary<string, object> Arguments { get; set; }
        
        [JsonPropertyName("exclusive")]
        public bool Exclusive { get; set; }
        
        [JsonPropertyName("auto_delete")]
        public bool AutoDelete { get; set; }
        
        [JsonPropertyName("durable")]
        public bool Durable { get; set; }
        
        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("node")]
        public string Node { get; set; }
        
        [JsonPropertyName("message_bytes_paged_out")]
        public ulong MessageBytesPagedOut { get; set; }
        
        [JsonPropertyName("messages_paged_out")]
        public ulong TotalMessagesPagedOut { get; set; }
        
        [JsonPropertyName("backing_queue_status")]
        public BackingQueueStatusImpl BackingQueueStatus { get; set; }
        
        [JsonPropertyName("head_message_timestamp")]
        public DateTimeOffset HeadMessageTimestamp { get; set; }
        
        [JsonPropertyName("message_bytes_persistent")]
        public ulong MessageBytesPersisted { get; set; }
        
        [JsonPropertyName("message_bytes_ram")]
        public ulong MessageBytesInRAM { get; set; }
        
        [JsonPropertyName("message_bytes_unacknowledged")]
        public ulong TotalBytesOfMessagesDeliveredButUnacknowledged { get; set; }
        
        [JsonPropertyName("message_bytes_ready")]
        public ulong TotalBytesOfMessagesReadyForDelivery { get; set; }
        
        [JsonPropertyName("message_bytes")]
        public ulong TotalBytesOfAllMessages { get; set; }
        
        [JsonPropertyName("messages_persistent")]
        public ulong MessagesPersisted { get; set; }
        
        [JsonPropertyName("messages_unacknowledged_ram")]
        public ulong UnacknowledgedMessagesInRAM { get; set; }
        
        [JsonPropertyName("messages_ready_ram")]
        public ulong MessagesReadyForDeliveryInRAM { get; set; }
        
        [JsonPropertyName("messages_ram")]
        public ulong MessagesInRAM { get; set; }
        
        [JsonPropertyName("garbage_collection")]
        public GarbageCollectionDetailsImpl GC { get; set; }
        
        [JsonPropertyName("state")]
        public QueueState State { get; set; }
        
        [JsonPropertyName("recoverable_slaves")]
        public IList<string> RecoverableSlaves { get; set; }
        
        [JsonPropertyName("consumers")]
        public ulong Consumers { get; set; }
        
        [JsonPropertyName("exclusive_consumer_tag")]
        public string ExclusiveConsumerTag { get; set; }
        
        [JsonPropertyName("policy")]
        public string Policy { get; set; }
        
        [JsonPropertyName("consumer_utilisation")]
        public decimal ConsumerUtilization { get; set; }
        
        [JsonPropertyName("idle_since")]
        public DateTimeOffset IdleSince { get; set; }
        
        [JsonPropertyName("memory")]
        public ulong Memory { get; set; }
        
        [JsonPropertyName("message_stats")]
        public QueueMessageStatsImpl MessageStats { get; set; }
    }
}
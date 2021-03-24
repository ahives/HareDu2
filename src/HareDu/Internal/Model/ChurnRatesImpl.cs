namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class ChurnRatesImpl
    {
        [JsonPropertyName("channel_closed")]
        public ulong TotalChannelsClosed { get; set; }
        
        [JsonPropertyName("channel_closed_details")]
        public RateImpl ClosedChannelDetails { get; set; }

        [JsonPropertyName("channel_created")]
        public ulong TotalChannelsCreated { get; set; }
        
        [JsonPropertyName("channel_created_details")]
        public RateImpl CreatedChannelDetails { get; set; }

        [JsonPropertyName("connection_closed")]
        public ulong TotalConnectionsClosed { get; set; }
        
        [JsonPropertyName("connection_closed_details")]
        public RateImpl ClosedConnectionDetails { get; set; }

        [JsonPropertyName("connection_created")]
        public ulong TotalConnectionsCreated { get; set; }
        
        [JsonPropertyName("connection_created_details")]
        public RateImpl CreatedConnectionDetails { get; set; }

        [JsonPropertyName("queue_created")]
        public ulong TotalQueuesCreated { get; set; }
        
        [JsonPropertyName("queue_created_details")]
        public RateImpl CreatedQueueDetails { get; set; }

        [JsonPropertyName("queue_declared")]
        public ulong TotalQueuesDeclared { get; set; }
        
        [JsonPropertyName("queue_declared_details")]
        public RateImpl DeclaredQueueDetails { get; set; }

        [JsonPropertyName("queue_deleted")]
        public ulong TotalQueuesDeleted { get; set; }
        
        [JsonPropertyName("queue_deleted_details")]
        public RateImpl DeletedQueueDetails { get; set; }
    }
}
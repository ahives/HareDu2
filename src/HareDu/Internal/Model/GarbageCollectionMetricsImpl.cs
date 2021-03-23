namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class GarbageCollectionMetricsImpl
    {
        [JsonPropertyName("connection_closed")]
        public ulong ConnectionsClosed { get; set; }

        [JsonPropertyName("channel_closed")]
        public ulong ChannelsClosed { get; set; }

        [JsonPropertyName("consumer_deleted")]
        public ulong ConsumersDeleted { get; set; }
        
        [JsonPropertyName("exchange_deleted")]
        public ulong ExchangesDeleted { get; set; }

        [JsonPropertyName("queue_deleted")]
        public ulong QueuesDeleted { get; set; }

        [JsonPropertyName("vhost_deleted")]
        public ulong VirtualHostsDeleted { get; set; }

        [JsonPropertyName("node_node_deleted")]
        public ulong NodesDeleted { get; set; }

        [JsonPropertyName("channel_consumer_deleted")]
        public ulong ChannelConsumersDeleted { get; set; }
    }
}
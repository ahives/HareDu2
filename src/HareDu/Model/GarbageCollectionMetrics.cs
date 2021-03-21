namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface GarbageCollectionMetrics
    {
        [JsonPropertyName("connection_closed")]
        ulong ConnectionsClosed { get; }

        [JsonPropertyName("channel_closed")]
        ulong ChannelsClosed { get; }

        [JsonPropertyName("consumer_deleted")]
        ulong ConsumersDeleted { get; }
        
        [JsonPropertyName("exchange_deleted")]
        ulong ExchangesDeleted { get; }

        [JsonPropertyName("queue_deleted")]
        ulong QueuesDeleted { get; }

        [JsonPropertyName("vhost_deleted")]
        ulong VirtualHostsDeleted { get; }

        [JsonPropertyName("node_node_deleted")]
        ulong NodesDeleted { get; }

        [JsonPropertyName("channel_consumer_deleted")]
        ulong ChannelConsumersDeleted { get; }
    }
}
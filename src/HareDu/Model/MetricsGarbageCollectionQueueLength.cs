namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface MetricsGarbageCollectionQueueLength
    {
        [JsonProperty("connection_closed")]
        long ConnectionsClosed { get; }

        [JsonProperty("channel_closed")]
        long ChannelsClosed { get; }

        [JsonProperty("consumer_deleted")]
        long ConsumersDeleted { get; }
        
        [JsonProperty("exchange_deleted")]
        long ExchangesDeleted { get; }

        [JsonProperty("queue_deleted")]
        long QueuesDeleted { get; }

        [JsonProperty("vhost_deleted")]
        long VirtualHostsDeleted { get; }

        [JsonProperty("node_node_deleted")]
        long NodesDeleted { get; }

        [JsonProperty("channel_consumer_deleted")]
        long ChannelConsumersDeleted { get; }
    }
}
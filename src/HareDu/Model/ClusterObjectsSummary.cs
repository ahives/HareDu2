namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface ClusterObjectsSummary
    {
        [JsonProperty("consumers")]
        long TotalConsumers { get; }
        
        [JsonProperty("queues")]
        long TotalQueues { get; }
        
        [JsonProperty("exchanges")]
        long TotalExchanges { get; }
        
        [JsonProperty("connections")]
        long TotalConnections { get; }
        
        [JsonProperty("channels")]
        long TotalChannels { get; }
    }
}
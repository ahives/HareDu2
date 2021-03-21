namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface ClusterObjectTotals
    {
        [JsonPropertyName("consumers")]
        ulong TotalConsumers { get; }
        
        [JsonPropertyName("queues")]
        ulong TotalQueues { get; }
        
        [JsonPropertyName("exchanges")]
        ulong TotalExchanges { get; }
        
        [JsonPropertyName("connections")]
        ulong TotalConnections { get; }
        
        [JsonPropertyName("channels")]
        ulong TotalChannels { get; }
    }
}
namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class ClusterObjectTotalsImpl
    {
        [JsonPropertyName("consumers")]
        public ulong TotalConsumers { get; set; }
        
        [JsonPropertyName("queues")]
        public ulong TotalQueues { get; set; }
        
        [JsonPropertyName("exchanges")]
        public ulong TotalExchanges { get; set; }
        
        [JsonPropertyName("connections")]
        public ulong TotalConnections { get; set; }
        
        [JsonPropertyName("channels")]
        public ulong TotalChannels { get; set; }
    }
}
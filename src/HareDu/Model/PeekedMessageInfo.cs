namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface PeekedMessageInfo
    {
        [JsonPropertyName("payload_bytes")]
        ulong PayloadBytes { get; }
        
        [JsonPropertyName("redelivered")]
        bool Redelivered { get; }
        
        [JsonPropertyName("exchange")]
        string Exchange { get; }
        
        [JsonPropertyName("routing_key")]
        string RoutingKey { get; }
        
        [JsonPropertyName("message_count")]
        ulong MessageCount { get; }
        
        [JsonPropertyName("properties")]
        PeekedMessageProperties Properties { get; }
        
        [JsonPropertyName("payload")]
        IDictionary<string, object> Payload { get; }
        
        [JsonPropertyName("payload_encoding")]
        string PayloadEncoding { get; }
    }
}
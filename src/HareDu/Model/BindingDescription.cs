namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface BindingDescription
    {
        [JsonPropertyName("source")]
        string Source { get; }
        
        [JsonPropertyName("destination")]
        string Destination { get; }
        
        [JsonPropertyName("destination_type")]
        string DestinationType { get; }
        
        [JsonPropertyName("routing_key")]
        string RoutingKey { get; }
        
        [JsonPropertyName("arguments")]
        IDictionary<string, string> Arguments { get; }
    }
}
namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface BindingSummary
    {
        [JsonProperty("source")]
        string Source { get; }
        
        [JsonProperty("destination")]
        string Destination { get; }
        
        [JsonProperty("destination_type")]
        string DestinationType { get; }
        
        [JsonProperty("routing_key")]
        string RoutingKey { get; }
        
        [JsonProperty("arguments")]
        IDictionary<string, string> Arguments { get; }
    }
}
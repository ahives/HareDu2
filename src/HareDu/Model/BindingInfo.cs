namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface BindingInfo
    {
        [JsonProperty("source")]
        string Source { get; }
        
        [JsonProperty("vhost")]
        string VirtualHost { get; }
        
        [JsonProperty("destination")]
        string Destination { get; }
        
        [JsonProperty("destination_type")]
        string DestinationType { get; }
        
        [JsonProperty("routing_key")]
        string RoutingKey { get; }
        
        [JsonProperty("arguments")]
        IDictionary<string, object> Arguments { get; }
        
        [JsonProperty("properties_key")]
        string PropertiesKey { get; }
    }
}
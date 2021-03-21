namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using HareDu.Model;

    class BindingInfoImpl :
        BindingInfo
    {
        [JsonPropertyName("source")]
        public string Source { get; set; }
        
        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }
        
        [JsonPropertyName("destination")]
        public string Destination { get; set; }
        
        [JsonPropertyName("destination_type")]
        public string DestinationType { get; set; }
        
        [JsonPropertyName("routing_key")]
        public string RoutingKey { get; set; }
        
        [JsonPropertyName("arguments")]
        public IDictionary<string, object> Arguments { get; set; }
        
        [JsonPropertyName("properties_key")]
        public string PropertiesKey { get; set; }
    }
}
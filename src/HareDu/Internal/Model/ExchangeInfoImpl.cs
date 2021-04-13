namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    class ExchangeInfoImpl
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }
        
        [JsonPropertyName("type")]
        public ExchangeRoutingType RoutingType { get; set; }
        
        [JsonPropertyName("durable")]
        public bool Durable { get; set; }
        
        [JsonPropertyName("auto_delete")]
        public bool AutoDelete { get; set; }
        
        [JsonPropertyName("internal")]
        public bool Internal { get; set; }
        
        [JsonPropertyName("arguments")]
        public IDictionary<string, object> Arguments { get; set; }
    }
}
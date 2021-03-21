namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface ExchangeInfo
    {
        [JsonPropertyName("name")]
        string Name { get; }
        
        [JsonPropertyName("vhost")]
        string VirtualHost { get; }
        
        [JsonPropertyName("type")]
        string RoutingType { get; }
        
        [JsonPropertyName("durable")]
        bool Durable { get; }
        
        [JsonPropertyName("auto_delete")]
        bool AutoDelete { get; }
        
        [JsonPropertyName("internal")]
        bool Internal { get; }
        
        [JsonPropertyName("arguments")]
        IDictionary<string, object> Arguments { get; }
    }
}
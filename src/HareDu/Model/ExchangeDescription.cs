namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface ExchangeDescription
    {
        [JsonPropertyName("name")]
        string Name { get; }
        
        [JsonPropertyName("type")]
        string Type { get; }
        
        [JsonPropertyName("durable")]
        bool Durable { get; }
        
        [JsonPropertyName("auto_delete")]
        bool AutoDelete { get; }
        
        [JsonPropertyName("internal")]
        bool Internal { get; }
        
        [JsonPropertyName("arguments")]
        IDictionary<string, string> Arguments { get; }
    }
}
namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface QueueDefinition
    {
        [JsonPropertyName("node")]
        string Node { get; }
        
        [JsonPropertyName("durable")]
        bool Durable { get; }
        
        [JsonPropertyName("auto_delete")]
        bool AutoDelete { get; }
                
        [JsonPropertyName("arguments")]
        IDictionary<string, object> Arguments { get; }
    }
}
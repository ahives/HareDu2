namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface QueueRequest
    {
        [JsonPropertyName("node")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string Node { get; }
        
        [JsonPropertyName("durable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        bool Durable { get; }
        
        [JsonPropertyName("auto_delete")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        bool AutoDelete { get; }
                
        [JsonPropertyName("arguments")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        IDictionary<string, object> Arguments { get; }
    }
}
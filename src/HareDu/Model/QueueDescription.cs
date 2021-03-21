namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface QueueDescription
    {
        [JsonPropertyName("name")]
        string Name { get; }

        [JsonPropertyName("durable")]
        bool IsDurable { get; }

        [JsonPropertyName("auto_delete")]
        bool IsAutoDelete { get; }

        [JsonPropertyName("arguments")]
        IDictionary<string, object> Arguments { get; }
    }
}
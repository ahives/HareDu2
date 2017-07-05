namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface QueueSummary
    {
        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("durable")]
        bool IsDurable { get; }

        [JsonProperty("auto_delete")]
        bool IsAutoDelete { get; }

        [JsonProperty("arguments")]
        IDictionary<string, string> Arguments { get; }
    }
}
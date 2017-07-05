namespace HareDu.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface ExchangeSummary
    {
        [JsonProperty("name")]
        string Name { get; }
        
        [JsonProperty("type")]
        string Type { get; }
        
        [JsonProperty("durable")]
        bool Durable { get; }
        
        [JsonProperty("auto_delete")]
        bool AutoDelete { get; }
        
        [JsonProperty("internal")]
        bool Internal { get; }
        
        [JsonProperty("arguments")]
        IDictionary<string, string> Arguments { get; }
    }
}
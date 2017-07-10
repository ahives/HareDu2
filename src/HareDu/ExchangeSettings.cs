namespace HareDu
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface ExchangeSettings
    {
        [JsonProperty("type")]
        string RoutingType { get; }
        
        [JsonProperty("durable")]
        bool Durable { get; }
        
        [JsonProperty("auto_delete")]
        bool AutoDelete { get; }
        
        [JsonProperty("internal")]
        bool Internal { get; }
        
        [JsonProperty("arguments", Required = Required.Default)]
        IDictionary<string, object> Arguments { get; }
    }
}
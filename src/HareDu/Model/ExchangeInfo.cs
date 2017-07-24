namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface ExchangeInfo
    {
        [JsonProperty("name")]
        string Name { get; }
        
        [JsonProperty("vhost")]
        string VirtualHost { get; }
        
        [JsonProperty("type")]
        string RoutingType { get; }
        
        [JsonProperty("durable")]
        bool Durable { get; }
        
        [JsonProperty("auto_delete")]
        bool AutoDelete { get; }
        
        [JsonProperty("internal")]
        bool Internal { get; }
    }
}
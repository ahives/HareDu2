namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface VirtualHost
    {
        [JsonProperty("name")]
        string Name { get; }
        
        [JsonProperty("tracing")]
        string Tracing { get; }
    }
}
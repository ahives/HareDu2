namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface VirtualHostInfo :
        ResourceSummary
    {
        [JsonProperty("name")]
        string Name { get; }
        
        [JsonProperty("tracing")]
        string Tracing { get; }
    }
}
namespace HareDu.Internal.Model
{
    using Newtonsoft.Json;

    public class VirtualHostSummary
    {
        [JsonProperty("name")]
        string Name { get; set; }
        
        [JsonProperty("tracing")]
        string Tracing { get; set; }

    }
}
namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface VirtualHostHealthCheck :
        ResourceSummary
    {
        [JsonProperty("status")]
        string Status { get; }
    }
}
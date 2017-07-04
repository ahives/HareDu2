namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface NodeHealthCheck
    {
        [JsonProperty("status")]
        string Status { get; }
        
        [JsonProperty("reason")]
        string Reason { get; }
    }
}
namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface PolicyDefinition
    {
        [JsonProperty("ha-mode")]
        string HighAvailabilityMode { get; }
        
        [JsonProperty("ha-params")]
        long HighAvailabilityParams { get; }
        
        [JsonProperty("ha-sync-mode")]
        string HighAvailabilitySyncMode { get; }
    }
}
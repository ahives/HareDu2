namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface PolicyDetails
    {
        [JsonPropertyName("ha-mode")]
        string HighAvailabilityMode { get; }
        
        [JsonPropertyName("ha-params")]
        long HighAvailabilityParams { get; }
        
        [JsonPropertyName("ha-sync-mode")]
        string HighAvailabilitySyncMode { get; }
    }
}
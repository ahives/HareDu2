namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface ServerHealthInfo
    {
        [JsonPropertyName("status")]
        string Status { get; }
        
        [JsonPropertyName("reason")]
        string Reason { get; }
    }
}
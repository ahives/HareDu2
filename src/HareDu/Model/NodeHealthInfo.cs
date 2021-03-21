namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface NodeHealthInfo
    {
        [JsonPropertyName("status")]
        string Status { get; }

        [JsonPropertyName("reason")]
        long Reason { get; }
    }
}
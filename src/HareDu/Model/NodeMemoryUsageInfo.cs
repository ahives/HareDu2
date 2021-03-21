namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface NodeMemoryUsageInfo
    {
        [JsonPropertyName("memory")]
        MemoryInfo Memory { get; }
    }
}
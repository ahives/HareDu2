namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface TotalMemoryInfo
    {
        [JsonPropertyName("erlang")]
        long Erlang { get; }
        
        [JsonPropertyName("rss")]
        long Strategy { get; }
        
        [JsonPropertyName("allocated")]
        long Allocated { get; }
    }
}
namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface QueuePeekDefinition
    {
        [JsonPropertyName("count")]
        uint Take { get; }
        
        [JsonPropertyName("encoding")]
        string Encoding { get; }
        
        [JsonPropertyName("truncate")]
        ulong TruncateMessageThreshold { get; }
        
        [JsonPropertyName("ackmode")]
        string RequeueMode { get; }
    }
}
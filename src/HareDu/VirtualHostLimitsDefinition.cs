namespace HareDu
{
    using System.Text.Json.Serialization;

    public interface VirtualHostLimitsDefinition
    {
        [JsonPropertyName("max-connections")]
        ulong MaxConnectionLimit { get; }
        
        [JsonPropertyName("max-queues")]
        ulong MaxQueueLimit { get; }
    }
}
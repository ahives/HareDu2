namespace HareDu
{
    using System.Text.Json.Serialization;

    public interface TopicPermissionsDefinition
    {
        [JsonPropertyName("exchange")]
        string Exchange { get; }
        
        [JsonPropertyName("write")]
        string Write { get; }
        
        [JsonPropertyName("read")]
        string Read { get; }
    }
}
namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface NodeContext
    {
        [JsonPropertyName("description")]
        string Description { get; }

        [JsonPropertyName("path")]
        string Path { get; }
        
        [JsonPropertyName("port")]
        string Port { get; }
    }
}
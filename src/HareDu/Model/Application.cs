namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface Application
    {
        [JsonPropertyName("name")]
        string Name { get; }

        [JsonPropertyName("description")]
        string Description { get; }

        [JsonPropertyName("version")]
        string Version { get; }
    }
}
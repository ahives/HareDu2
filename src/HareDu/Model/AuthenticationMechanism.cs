namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface AuthenticationMechanism
    {
        [JsonPropertyName("name")]
        string Name { get; }

        [JsonPropertyName("description")]
        string Description { get; }

        [JsonPropertyName("enabled")]
        bool IsEnabled { get; }
    }
}
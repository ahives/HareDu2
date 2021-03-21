namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface ExchangeType
    {
        [JsonPropertyName("name")]
        string Name { get; }

        [JsonPropertyName("description")]
        string Description { get; }

        [JsonPropertyName("enabled")]
        bool IsEnabled { get; }
    }
}
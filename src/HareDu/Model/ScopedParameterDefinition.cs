namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface ScopedParameterDefinition<out T>
    {
        [JsonPropertyName("vhost")]
        string VirtualHost { get; }

        [JsonPropertyName("component")]
        string Component { get; }

        [JsonPropertyName("name")]
        string ParameterName { get; }

        [JsonPropertyName("value")]
        T ParameterValue { get; }
    }
}
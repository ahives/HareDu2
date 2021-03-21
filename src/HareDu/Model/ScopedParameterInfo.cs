namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface ScopedParameterInfo
    {
        [JsonPropertyName("vhost")]
        string VirtualHost { get; }

        [JsonPropertyName("component")]
        string Component { get; }

        [JsonPropertyName("name")]
        string Name { get; }

        [JsonPropertyName("value")]
        IDictionary<string, object> Value { get; }
    }
}
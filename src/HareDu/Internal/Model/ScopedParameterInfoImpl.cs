namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    class ScopedParameterInfoImpl
    {
        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }

        [JsonPropertyName("component")]
        public string Component { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public IDictionary<string, object> Value { get; set; }
    }
}
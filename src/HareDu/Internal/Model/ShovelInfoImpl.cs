namespace HareDu.Internal.Model
{
    using System;
    using System.Text.Json.Serialization;
    using HareDu.Model;

    class ShovelInfoImpl :
        ShovelInfo
    {
        [JsonPropertyName("node")]
        public string Node { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }

        [JsonPropertyName("type")]
        public ShovelType Type { get; set; }

        [JsonPropertyName("state")]
        public ShovelState State { get; set; }
    }
}
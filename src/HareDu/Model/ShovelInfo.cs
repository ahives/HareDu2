namespace HareDu.Model
{
    using System;
    using Newtonsoft.Json;

    public interface ShovelInfo
    {
        [JsonProperty("node")]
        string Node { get; }

        [JsonProperty("timestamp")]
        DateTimeOffset Timestamp { get; }

        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("vhost")]
        string VirtualHost { get; }

        [JsonProperty("type")]
        string Type { get; }

        [JsonProperty("state")]
        string State { get; }
    }
}
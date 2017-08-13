namespace HareDu
{
    using Newtonsoft.Json;

    public interface GlobalParameterInfo
    {
        [JsonProperty("vhost")]
        string VirtualHost { get; }

        [JsonProperty("component")]
        string Component { get; }

        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("value")]
        string Value { get; }
    }
}
namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface ExchangeType
    {
        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("description")]
        string Description { get; }

        [JsonProperty("enabled")]
        bool Enabled { get; }
    }
}
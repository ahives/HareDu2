namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface Application
    {
        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("description")]
        string Description { get; }

        [JsonProperty("version")]
        string Version { get; }
    }
}
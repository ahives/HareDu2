namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface Context
    {
        [JsonProperty("node")]
        string Node { get; }

        [JsonProperty("description")]
        string Description { get; }

        [JsonProperty("path")]
        string Path { get; }
        
        [JsonProperty("port")]
        string Port { get; }
    }
}
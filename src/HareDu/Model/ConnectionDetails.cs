namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface ConnectionDetails
    {
        [JsonProperty("peer_host")]
        string PeerHost { get; }

        [JsonProperty("peer_port")]
        long PeerPort { get; }

        [JsonProperty("name")]
        string Name { get; }
    }
}
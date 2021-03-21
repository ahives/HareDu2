namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface ConnectionDetails
    {
        [JsonPropertyName("peer_host")]
        string PeerHost { get; }

        [JsonPropertyName("peer_port")]
        long PeerPort { get; }

        [JsonPropertyName("name")]
        string Name { get; }
    }
}
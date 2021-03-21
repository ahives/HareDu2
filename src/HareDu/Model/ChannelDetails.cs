namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface ChannelDetails
    {
        [JsonPropertyName("peer_host")]
        string PeerHost { get; }
        
        [JsonPropertyName("peer_port")]
        long PeerPort { get; }
        
        [JsonPropertyName("number")]
        long Number { get; }

        [JsonPropertyName("name")]
        string Name { get; }

        [JsonPropertyName("node")]
        string Node { get; }

        [JsonPropertyName("connection_name")]
        string ConnectionName { get; }

        [JsonPropertyName("user")]
        string User { get; }
    }
}
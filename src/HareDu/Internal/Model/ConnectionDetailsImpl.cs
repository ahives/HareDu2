namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class ConnectionDetailsImpl
    {
        [JsonPropertyName("peer_host")]
        public string PeerHost { get; set; }

        [JsonPropertyName("peer_port")]
        public long PeerPort { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
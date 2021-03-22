namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class ChannelDetailsImpl
    {
        [JsonPropertyName("peer_host")]
        public string PeerHost { get; set; }
        
        [JsonPropertyName("peer_port")]
        public long PeerPort { get; set; }
        
        [JsonPropertyName("number")]
        public long Number { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("node")]
        public string Node { get; set; }

        [JsonPropertyName("connection_name")]
        public string ConnectionName { get; set; }

        [JsonPropertyName("user")]
        public string User { get; set; }
    }
}
namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    class VirtualHostInfoImpl
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("tracing")]
        public bool Tracing { get; set; }
        
        [JsonPropertyName("cluster_state")]
        public IDictionary<string, string> ClusterState { get; set; }

        [JsonPropertyName("message_stats")]
        public VirtualHostMessageStatsImpl MessageStats { get; set; }

        [JsonPropertyName("recv_oct")]
        public ulong PacketBytesReceived { get; set; }

        [JsonPropertyName("recv_oct_details")]
        public RateImpl PacketBytesReceivedDetails { get; set; }

        [JsonPropertyName("send_oct")]
        public ulong PacketBytesSent { get; set; }

        [JsonPropertyName("send_oct_details")]
        public RateImpl PacketBytesSentDetails { get; set; }
        
        [JsonPropertyName("messages_details")]
        public RateImpl MessagesDetails { get; set; }
        
        [JsonPropertyName("messages")]
        public ulong TotalMessages { get; set; }
        
        [JsonPropertyName("messages_unacknowledged_details")]
        public RateImpl UnacknowledgedMessagesDetails { get; set; }
        
        [JsonPropertyName("messages_unacknowledged")]
        public ulong UnacknowledgedMessages { get; set; }
        
        [JsonPropertyName("messages_ready_details")]
        public RateImpl ReadyMessagesDetails { get; set; }
        
        [JsonPropertyName("messages_ready")]
        public ulong ReadyMessages { get; set; }
    }
}
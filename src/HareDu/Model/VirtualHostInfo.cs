namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public interface VirtualHostInfo
    {
        [JsonPropertyName("name")]
        string Name { get; }
        
        [JsonPropertyName("tracing")]
        string Tracing { get; }
        
        [JsonPropertyName("cluster_state")]
        IDictionary<string, string> ClusterState { get; }

        [JsonPropertyName("message_stats")]
        VirtualHostMessageStats MessageStats { get; }

        [JsonPropertyName("recv_oct")]
        ulong PacketBytesReceived { get; }

        [JsonPropertyName("recv_oct_details")]
        Rate PacketBytesReceivedDetails { get; }

        [JsonPropertyName("send_oct")]
        ulong PacketBytesSent { get; }

        [JsonPropertyName("send_oct_details")]
        Rate PacketBytesSentDetails { get; }
        
        [JsonPropertyName("messages_details")]
        Rate MessagesDetails { get; }
        
        [JsonPropertyName("messages")]
        ulong TotalMessages { get; }
        
        [JsonPropertyName("messages_unacknowledged_details")]
        Rate UnacknowledgedMessagesDetails { get; }
        
        [JsonPropertyName("messages_unacknowledged")]
        ulong UnacknowledgedMessages { get; }
        
        [JsonPropertyName("messages_ready_details")]
        Rate ReadyMessagesDetails { get; }
        
        [JsonPropertyName("messages_ready")]
        ulong ReadyMessages { get; }
    }
}
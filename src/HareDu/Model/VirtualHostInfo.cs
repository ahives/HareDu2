namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface VirtualHostInfo
    {
        string Name { get; }
        
        bool Tracing { get; }
        
        IDictionary<string, string> ClusterState { get; }

        VirtualHostMessageStats MessageStats { get; }

        ulong PacketBytesReceived { get; }

        Rate PacketBytesReceivedDetails { get; }

        ulong PacketBytesSent { get; }

        Rate PacketBytesSentDetails { get; }
        
        Rate MessagesDetails { get; }
        
        ulong TotalMessages { get; }
        
        Rate UnacknowledgedMessagesDetails { get; }
        
        ulong UnacknowledgedMessages { get; }
        
        Rate ReadyMessagesDetails { get; }
        
        ulong ReadyMessages { get; }
    }
}
namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using Core.Extensions;
    using HareDu.Model;

    class InternalVirtualHostInfo :
        VirtualHostInfo
    {
        public InternalVirtualHostInfo(VirtualHostInfoImpl obj)
        {
            Name = obj.Name;
            Tracing = obj.Tracing;
            ClusterState = obj.ClusterState;
            MessageStats = obj.MessageStats.IsNotNull() ? new InternalVirtualHostMessageStats(obj.MessageStats) : default;
            PacketBytesReceived = obj.PacketBytesReceived;
            PacketBytesReceivedDetails = obj.PacketBytesReceivedDetails.IsNotNull() ? new InternalRate(obj.PacketBytesReceivedDetails) : default;
            PacketBytesSent = obj.PacketBytesSent;
            TotalMessages = obj.TotalMessages;
            UnacknowledgedMessages = obj.UnacknowledgedMessages;
            ReadyMessages = obj.ReadyMessages;
            PacketBytesSentDetails = obj.PacketBytesSentDetails.IsNotNull() ? new InternalRate(obj.PacketBytesSentDetails) : default;
            MessagesDetails = obj.MessagesDetails.IsNotNull() ? new InternalRate(obj.MessagesDetails) : default;
            UnacknowledgedMessagesDetails = obj.UnacknowledgedMessagesDetails.IsNotNull() ? new InternalRate(obj.UnacknowledgedMessagesDetails) : default;
            ReadyMessagesDetails = obj.ReadyMessagesDetails.IsNotNull() ? new InternalRate(obj.ReadyMessagesDetails) : default;
        }

        public string Name { get; }
        public string Tracing { get; }
        public IDictionary<string, string> ClusterState { get; }
        public VirtualHostMessageStats MessageStats { get; }
        public ulong PacketBytesReceived { get; }
        public Rate PacketBytesReceivedDetails { get; }
        public ulong PacketBytesSent { get; }
        public Rate PacketBytesSentDetails { get; }
        public Rate MessagesDetails { get; }
        public ulong TotalMessages { get; }
        public Rate UnacknowledgedMessagesDetails { get; }
        public ulong UnacknowledgedMessages { get; }
        public Rate ReadyMessagesDetails { get; }
        public ulong ReadyMessages { get; }
    }
}
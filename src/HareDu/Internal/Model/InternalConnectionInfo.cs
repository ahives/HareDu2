namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalConnectionInfo :
        ConnectionInfo
    {
        public InternalConnectionInfo(ConnectionInfoImpl obj)
        {
            ReductionDetails = obj.ReductionDetails.IsNotNull() ? new InternalRateImpl(obj.ReductionDetails) : default;
            Protocol = obj.Protocol;
            TotalReductions = obj.TotalReductions;
            PacketsReceived = obj.PacketsReceived;
            PacketBytesReceived = obj.PacketBytesReceived;
            PacketBytesReceivedDetails = obj.PacketBytesReceivedDetails.IsNotNull() ? new InternalRateImpl(obj.PacketBytesReceivedDetails) : default;
            PacketsSent = obj.PacketsSent;
            PacketBytesSent = obj.PacketBytesSent;
            PacketBytesSentDetails = obj.PacketBytesSentDetails.IsNotNull() ? new InternalRateImpl(obj.PacketBytesSentDetails) : default;
            ConnectedAt = obj.ConnectedAt;
            OpenChannelsLimit = obj.OpenChannelsLimit;
            MaxFrameSizeInBytes = obj.MaxFrameSizeInBytes;
            ConnectionTimeout = obj.ConnectionTimeout;
            VirtualHost = obj.VirtualHost;
            Name = obj.Name;
            Channels = obj.Channels;
            SendPending = obj.SendPending;
            Type = obj.Type;
            GarbageCollectionDetails = obj.GarbageCollectionDetails.IsNotNull() ? new InternalGarbageCollectionDetails(obj.GarbageCollectionDetails) : default;
            State = obj.State;
            SslHashFunction = obj.SslHashFunction;
            SslCipherAlgorithm = obj.SslCipherAlgorithm;
            SslKeyExchangeAlgorithm = obj.SslKeyExchangeAlgorithm;
            SslProtocol = obj.SslProtocol;
            AuthenticationMechanism = obj.AuthenticationMechanism;
            TimePeriodPeerCertificateValid = obj.TimePeriodPeerCertificateValid;
            PeerCertificateIssuer = obj.PeerCertificateIssuer;
            PeerCertificateSubject = obj.PeerCertificateSubject;
            IsSsl = obj.IsSsl;
            PeerHost = obj.PeerHost;
            Host = obj.Host;
            PeerPort = obj.PeerPort;
            Port = obj.Port;
            Node = obj.Node;
            User = obj.User;
            UserWhoPerformedAction = obj.UserWhoPerformedAction;
            ConnectionClientProperties = obj.ConnectionClientProperties.IsNotNull() ? new InternalConnectionClientProperties(obj.ConnectionClientProperties) : default;
        }

        public Rate ReductionDetails { get; }
        public string Protocol { get; }
        public ulong TotalReductions { get; }
        public ulong PacketsReceived { get; }
        public ulong PacketBytesReceived { get; }
        public Rate PacketBytesReceivedDetails { get; }
        public ulong PacketsSent { get; }
        public ulong PacketBytesSent { get; }
        public Rate PacketBytesSentDetails { get; }
        public long ConnectedAt { get; }
        public ulong OpenChannelsLimit { get; }
        public ulong MaxFrameSizeInBytes { get; }
        public long ConnectionTimeout { get; }
        public string VirtualHost { get; }
        public string Name { get; }
        public ulong Channels { get; }
        public ulong SendPending { get; }
        public string Type { get; }
        public GarbageCollectionDetails GarbageCollectionDetails { get; }
        public BrokerConnectionState State { get; }
        public string SslHashFunction { get; }
        public string SslCipherAlgorithm { get; }
        public string SslKeyExchangeAlgorithm { get; }
        public string SslProtocol { get; }
        public string AuthenticationMechanism { get; }
        public string TimePeriodPeerCertificateValid { get; }
        public string PeerCertificateIssuer { get; }
        public string PeerCertificateSubject { get; }
        public bool IsSsl { get; }
        public string PeerHost { get; }
        public string Host { get; }
        public long PeerPort { get; }
        public long Port { get; }
        public string Node { get; }
        public string User { get; }
        public string UserWhoPerformedAction { get; }
        public ConnectionClientProperties ConnectionClientProperties { get; }
    }
}
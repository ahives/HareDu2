namespace HareDu.Snapshotting.Tests.Fakes
{
    using HareDu.Model;

    public class FakeConnectionInfo :
        ConnectionInfo
    {
        public FakeConnectionInfo(int number, int node)
        {
            TotalReductions = 897274932;
            OpenChannelsLimit = 982738;
            MaxFrameSizeInBytes = 627378937423;
            VirtualHost = "TestVirtualHost";
            Name = $"Connection {number}";
            Node = $"Node {node}";
            Channels = 7687264882;
            SendPending = 686219897;
            PacketsSent = 871998847;
            PacketBytesSent = 83008482374;
            PacketsReceived = 68721979894793;
            State = BrokerConnectionState.Blocked;
        }

        public Rate ReductionDetails { get; }
        public string Protocol { get; }
        public ulong TotalReductions { get; }
        public ulong PacketBytesReceived { get; }
        public Rate PacketBytesReceivedDetails { get; }
        public Rate PacketBytesSentDetails { get; }
        public long ConnectedAt { get; }
        public ulong OpenChannelsLimit { get; }
        public ulong MaxFrameSizeInBytes { get; }
        public long ConnectionTimeout { get; }
        public string VirtualHost { get; }
        public string Name { get; }
        public ulong Channels { get; }
        public ulong SendPending { get; }
        public ulong PacketsSent { get; }
        public ulong PacketBytesSent { get; }
        public ulong PacketsReceived { get; }
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
namespace HareDu.Snapshotting.Tests.Fakes
{
    using System;
    using HareDu.Internal.Model;
    using HareDu.Model;

    public class FakeChannelInfo :
        ChannelInfo
    {
        public FakeChannelInfo(int channel, int node)
        {
            TotalReductions = 872634826473;
            VirtualHost = "TestVirtualHost";
            Node = $"Node {node}";
            FrameMax = 728349837;
            Name = $"Channel {channel}";
            TotalChannels = 87;
            SentPending = 89;
            PrefetchCount = 78;
            UncommittedAcknowledgements = 98237843;
            UncommittedMessages = 383902;
            UnconfirmedMessages = 82930;
            UnacknowledgedMessages = 7882003;
            TotalConsumers = 90;
            ConnectionDetails = new ConnectionDetailsImpl();
        }
        
        public FakeChannelInfo(int channel, int node, int connection)
        {
            TotalReductions = 872634826473;
            VirtualHost = "TestVirtualHost";
            Node = $"Node {node}";
            FrameMax = 728349837;
            Name = $"Channel {channel}";
            TotalChannels = 87;
            SentPending = 89;
            PrefetchCount = 78;
            UncommittedAcknowledgements = 98237843;
            UncommittedMessages = 383902;
            UnconfirmedMessages = 82930;
            UnacknowledgedMessages = 7882003;
            TotalConsumers = 90;
            ConnectionDetails = new ConnectionDetailsImpl(connection);
        }

        public Rate ReductionDetails { get; }
        public ulong TotalReductions { get; }
        public string VirtualHost { get; }
        public string Node { get; }
        public string User { get; }
        public string UserWhoPerformedAction { get; }
        public long ConnectedAt { get; }
        public ulong FrameMax { get; }
        public long Timeout { get; }
        public ulong Number { get; }
        public string Name { get; }
        public string Protocol { get; }
        public string SslHash { get; }
        public string SslCipher { get; }
        public string SslKeyExchange { get; }
        public string SslProtocol { get; }
        public string AuthenticationMechanism { get; }
        public string PeerCertificateValidity { get; }
        public string PeerCertificateIssuer { get; }
        public string PeerCertificateSubject { get; }
        public bool Ssl { get; }
        public string PeerHost { get; }
        public string Host { get; }
        public long PeerPort { get; }
        public long Port { get; }
        public string Type { get; }
        public ConnectionDetails ConnectionDetails { get; }
        public GarbageCollectionDetails GarbageCollectionDetails { get; }
        public ChannelState State { get; }
        public long TotalChannels { get; }
        public long SentPending { get; }
        public uint GlobalPrefetchCount { get; }
        public uint PrefetchCount { get; }
        public ulong UncommittedAcknowledgements { get; }
        public ulong UncommittedMessages { get; }
        public ulong UnconfirmedMessages { get; }
        public ulong UnacknowledgedMessages { get; }
        public ulong TotalConsumers { get; }
        public bool Confirm { get; }
        public bool Transactional { get; }
        public DateTimeOffset IdleSince { get; }
        public ChannelOperationStats OperationStats { get; }


        class ConnectionDetailsImpl :
            ConnectionDetails
        {
            public ConnectionDetailsImpl()
            {
                Name = "Connection 1";
            }

            public ConnectionDetailsImpl(int connection)
            {
                Name = $"Connection {connection}";
            }

            public string PeerHost { get; }
            public long PeerPort { get; }
            public string Name { get; }
        }
    }
}
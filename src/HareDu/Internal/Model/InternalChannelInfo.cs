namespace HareDu.Internal.Model
{
    using System;
    using Core.Extensions;
    using HareDu.Model;

    class InternalChannelInfo :
        ChannelInfo
    {
        public InternalChannelInfo(ChannelInfoImpl item)
        {
            ReductionDetails = item.ReductionDetails.IsNotNull() ? new InternalRateImpl(item.ReductionDetails) : null;
            TotalReductions = item.TotalReductions;
            VirtualHost = item.VirtualHost;
            Node = item.Node;
            User = item.User;
            UserWhoPerformedAction = item.UserWhoPerformedAction;
            ConnectedAt = item.ConnectedAt;
            FrameMax = item.FrameMax;
            Timeout = item.Timeout;
            Number = item.Number;
            Name = item.Name;
            Protocol = item.Protocol;
            SslHash = item.SslHash;
            SslCipher = item.SslCipher;
            SslKeyExchange = item.SslKeyExchange;
            SslProtocol = item.SslProtocol;
            AuthenticationMechanism = item.AuthenticationMechanism;
            PeerCertificateValidity = item.PeerCertificateValidity;
            PeerCertificateIssuer = item.PeerCertificateIssuer;
            PeerCertificateSubject = item.PeerCertificateSubject;
            Ssl = item.Ssl;
            PeerHost = item.PeerHost;
            Host = item.Host;
            PeerPort = item.PeerPort;
            Port = item.Port;
            Type = item.Type;
            ConnectionDetails = item.ConnectionDetails.IsNotNull() ? new InternalConnectionDetails(item.ConnectionDetails) : default;
            GarbageCollectionDetails = item.GarbageCollectionDetails.IsNotNull() ? new InternalGarbageCollectionDetails(item.GarbageCollectionDetails) : default;
            State = item.State;
            TotalChannels = item.TotalChannels;
            SentPending = item.SentPending;
            GlobalPrefetchCount = item.GlobalPrefetchCount;
            PrefetchCount = item.PrefetchCount;
            UncommittedAcknowledgements = item.UncommittedAcknowledgements;
            UncommittedMessages = item.UncommittedMessages;
            UnconfirmedMessages = item.UnconfirmedMessages;
            UnacknowledgedMessages = item.UnacknowledgedMessages;
            TotalConsumers = item.TotalConsumers;
            Confirm = item.Confirm;
            Transactional = item.Transactional;
            IdleSince = item.IdleSince;
            OperationStats = item.OperationStats.IsNotNull() ? new InternalOperationStatsImpl(item.OperationStats) : default;
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
        public string State { get; }
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
    }
}
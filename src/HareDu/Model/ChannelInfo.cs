namespace HareDu.Model
{
    using System;
    using Internal.Model;

    public interface ChannelInfo
    {
        Rate ReductionDetails { get; }

        ulong TotalReductions { get; }

        string VirtualHost { get; }

        string Node { get; }

        string User { get; }

        string UserWhoPerformedAction { get; }

        long ConnectedAt { get; }

        ulong FrameMax { get; }

        long Timeout { get; }

        ulong Number { get; }

        string Name { get; }

        string Protocol { get; }

        string SslHash { get; }

        string SslCipher { get; }

        string SslKeyExchange { get; }

        string SslProtocol { get; }

        string AuthenticationMechanism { get; }

        string PeerCertificateValidity { get; }

        string PeerCertificateIssuer { get; }

        string PeerCertificateSubject { get; }

        bool Ssl { get; }

        string PeerHost { get; }

        string Host { get; }

        long PeerPort { get; }

        long Port { get; }

        string Type { get; }

        ConnectionDetails ConnectionDetails { get; }

        GarbageCollectionDetails GarbageCollectionDetails { get; }

        ChannelState State { get; }

        long TotalChannels { get; }

        long SentPending { get; }

        uint GlobalPrefetchCount { get; }

        uint PrefetchCount { get; }

        ulong UncommittedAcknowledgements { get; }

        ulong UncommittedMessages { get; }

        ulong UnconfirmedMessages { get; }

        ulong UnacknowledgedMessages { get; }

        ulong TotalConsumers { get; }

        bool Confirm { get; }

        bool Transactional { get; }

        DateTimeOffset IdleSince { get; }
        
        ChannelOperationStats OperationStats { get; }
    }
}
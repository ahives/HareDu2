namespace HareDu.Model
{
    public interface ConnectionInfo
    {
        Rate ReductionDetails { get; }
        
        string Protocol { get; }

        ulong TotalReductions { get; }

        ulong PacketsReceived { get; }

        ulong PacketBytesReceived { get; }

        Rate PacketBytesReceivedDetails { get; }

        ulong PacketsSent { get; }

        ulong PacketBytesSent { get; }

        Rate PacketBytesSentDetails { get; }

        long ConnectedAt { get; }

        ulong OpenChannelsLimit { get; }

        ulong MaxFrameSizeInBytes { get; }

        long ConnectionTimeout { get; }

        string VirtualHost { get; }

        string Name { get; }

        ulong Channels { get; }

        ulong SendPending { get; }

        string Type { get; }

        GarbageCollectionDetails GarbageCollectionDetails { get; }

        string State { get; }

        string SslHashFunction { get; }

        string SslCipherAlgorithm { get; }

        string SslKeyExchangeAlgorithm { get; }

        string SslProtocol { get; }

        string AuthenticationMechanism { get; }

        string TimePeriodPeerCertificateValid { get; }

        string PeerCertificateIssuer { get; }

        string PeerCertificateSubject { get; }

        bool IsSsl { get; }

        string PeerHost { get; }

        string Host { get; }

        long PeerPort { get; }

        long Port { get; }

        string Node { get; }

        string User { get; }

        string UserWhoPerformedAction { get; }

        ConnectionClientProperties ConnectionClientProperties { get; }
    }
}
namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface ConnectionInfo
    {
        [JsonPropertyName("reductions_details")]
        Rate ReductionDetails { get; }
        
        [JsonPropertyName("protocol")]
        string Protocol { get; }

        [JsonPropertyName("reductions")]
        ulong TotalReductions { get; }

        [JsonPropertyName("recv_cnt")]
        ulong PacketsReceived { get; }

        [JsonPropertyName("recv_oct")]
        ulong PacketBytesReceived { get; }

        [JsonPropertyName("recv_oct_details")]
        Rate PacketBytesReceivedDetails { get; }

        [JsonPropertyName("send_cnt")]
        ulong PacketsSent { get; }

        [JsonPropertyName("send_oct")]
        ulong PacketBytesSent { get; }

        [JsonPropertyName("send_oct_details")]
        Rate PacketBytesSentDetails { get; }

        [JsonPropertyName("connected_at")]
        long ConnectedAt { get; }

        [JsonPropertyName("channel_max")]
        ulong OpenChannelsLimit { get; }

        [JsonPropertyName("frame_max")]
        ulong MaxFrameSizeInBytes { get; }

        [JsonPropertyName("timeout")]
        long ConnectionTimeout { get; }

        [JsonPropertyName("vhost")]
        string VirtualHost { get; }

        [JsonPropertyName("name")]
        string Name { get; }

        [JsonPropertyName("channels")]
        ulong Channels { get; }

        [JsonPropertyName("send_pend")]
        ulong SendPending { get; }

        [JsonPropertyName("type")]
        string Type { get; }

        [JsonPropertyName("garbage_collection")]
        GarbageCollectionDetails GarbageCollectionDetails { get; }

        [JsonPropertyName("state")]
        string State { get; }

        [JsonPropertyName("ssl_hash")]
        string SslHashFunction { get; }

        [JsonPropertyName("ssl_cipher")]
        string SslCipherAlgorithm { get; }

        [JsonPropertyName("ssl_key_exchange")]
        string SslKeyExchangeAlgorithm { get; }

        [JsonPropertyName("ssl_protocol")]
        string SslProtocol { get; }

        [JsonPropertyName("auth_mechanism")]
        string AuthenticationMechanism { get; }

        [JsonPropertyName("peer_cert_validity")]
        string TimePeriodPeerCertificateValid { get; }

        [JsonPropertyName("peer_cert_issuer")]
        string PeerCertificateIssuer { get; }

        [JsonPropertyName("peer_cert_subject")]
        string PeerCertificateSubject { get; }

        [JsonPropertyName("ssl")]
        bool IsSsl { get; }

        [JsonPropertyName("peer_host")]
        string PeerHost { get; }

        [JsonPropertyName("host")]
        string Host { get; }

        [JsonPropertyName("peer_port")]
        long PeerPort { get; }

        [JsonPropertyName("port")]
        long Port { get; }

        [JsonPropertyName("node")]
        string Node { get; }

        [JsonPropertyName("user")]
        string User { get; }

        [JsonPropertyName("user_who_performed_action")]
        string UserWhoPerformedAction { get; }

        [JsonPropertyName("client_properties")]
        ConnectionClientProperties ConnectionClientProperties { get; }
    }
}
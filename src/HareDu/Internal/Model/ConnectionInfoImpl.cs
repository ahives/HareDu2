namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;
    using HareDu.Model;

    class ConnectionInfoImpl
    {
        [JsonPropertyName("reductions_details")]
        public RateImpl ReductionDetails { get; set; }
        
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }

        [JsonPropertyName("reductions")]
        public ulong TotalReductions { get; set; }

        [JsonPropertyName("recv_cnt")]
        public ulong PacketsReceived { get; set; }

        [JsonPropertyName("recv_oct")]
        public ulong PacketBytesReceived { get; set; }

        [JsonPropertyName("recv_oct_details")]
        public RateImpl PacketBytesReceivedDetails { get; set; }

        [JsonPropertyName("send_cnt")]
        public ulong PacketsSent { get; set; }

        [JsonPropertyName("send_oct")]
        public ulong PacketBytesSent { get; set; }

        [JsonPropertyName("send_oct_details")]
        public RateImpl PacketBytesSentDetails { get; set; }

        [JsonPropertyName("connected_at")]
        public long ConnectedAt { get; set; }

        [JsonPropertyName("channel_max")]
        public ulong OpenChannelsLimit { get; set; }

        [JsonPropertyName("frame_max")]
        public ulong MaxFrameSizeInBytes { get; set; }

        [JsonPropertyName("timeout")]
        public long ConnectionTimeout { get; set; }

        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("channels")]
        public ulong Channels { get; set; }

        [JsonPropertyName("send_pend")]
        public ulong SendPending { get; set; }

        [JsonPropertyName("type")]
        public ConnectionType Type { get; set; }

        [JsonPropertyName("garbage_collection")]
        public GarbageCollectionDetailsImpl GarbageCollectionDetails { get; set; }

        [JsonPropertyName("state")]
        public BrokerConnectionState State { get; set; }

        [JsonPropertyName("ssl_hash")]
        public string SslHashFunction { get; set; }

        [JsonPropertyName("ssl_cipher")]
        public string SslCipherAlgorithm { get; set; }

        [JsonPropertyName("ssl_key_exchange")]
        public string SslKeyExchangeAlgorithm { get; set; }

        [JsonPropertyName("ssl_protocol")]
        public string SslProtocol { get; set; }

        [JsonPropertyName("auth_mechanism")]
        public string AuthenticationMechanism { get; set; }

        [JsonPropertyName("peer_cert_validity")]
        public string TimePeriodPeerCertificateValid { get; set; }

        [JsonPropertyName("peer_cert_issuer")]
        public string PeerCertificateIssuer { get; set; }

        [JsonPropertyName("peer_cert_subject")]
        public string PeerCertificateSubject { get; set; }

        [JsonPropertyName("ssl")]
        public bool IsSsl { get; set; }

        [JsonPropertyName("peer_host")]
        public string PeerHost { get; set; }

        [JsonPropertyName("host")]
        public string Host { get; set; }

        [JsonPropertyName("peer_port")]
        public long PeerPort { get; set; }

        [JsonPropertyName("port")]
        public long Port { get; set; }

        [JsonPropertyName("node")]
        public string Node { get; set; }

        [JsonPropertyName("user")]
        public string User { get; set; }

        [JsonPropertyName("user_who_performed_action")]
        public string UserWhoPerformedAction { get; set; }

        [JsonPropertyName("client_properties")]
        public ConnectionClientPropertiesImpl ConnectionClientProperties { get; set; }
    }
}
namespace HareDu.Internal.Model
{
    using System;
    using System.Text.Json.Serialization;

    class ChannelInfoImpl
    {
        [JsonPropertyName("reductions_details")]
        public RateImpl ReductionDetails { get; set; }

        [JsonPropertyName("reductions")]
        public ulong TotalReductions { get; set; }

        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }

        [JsonPropertyName("node")]
        public string Node { get; set; }

        [JsonPropertyName("user")]
        public string User { get; set; }

        [JsonPropertyName("user_who_performed_action")]
        public string UserWhoPerformedAction { get; set; }

        [JsonPropertyName("connected_at")]
        public long ConnectedAt { get; set; }

        [JsonPropertyName("frame_max")]
        public ulong FrameMax { get; set; }

        [JsonPropertyName("timeout")]
        public long Timeout { get; set; }

        [JsonPropertyName("number")]
        public ulong Number { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }

        [JsonPropertyName("ssl_hash")]
        public string SslHash { get; set; }

        [JsonPropertyName("ssl_cipher")]
        public string SslCipher { get; set; }

        [JsonPropertyName("ssl_key_exchange")]
        public string SslKeyExchange { get; set; }

        [JsonPropertyName("ssl_protocol")]
        public string SslProtocol { get; set; }

        [JsonPropertyName("auth_mechanism")]
        public string AuthenticationMechanism { get; set; }

        [JsonPropertyName("peer_cert_validity")]
        public string PeerCertificateValidity { get; set; }

        [JsonPropertyName("peer_cert_issuer")]
        public string PeerCertificateIssuer { get; set; }

        [JsonPropertyName("peer_cert_subject")]
        public string PeerCertificateSubject { get; set; }

        [JsonPropertyName("ssl")]
        public bool Ssl { get; set; }

        [JsonPropertyName("peer_host")]
        public string PeerHost { get; set; }

        [JsonPropertyName("host")]
        public string Host { get; set; }

        [JsonPropertyName("peer_port")]
        public long PeerPort { get; set; }

        [JsonPropertyName("port")]
        public long Port { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("connection_details")]
        public ConnectionDetailsImpl ConnectionDetails { get; set; }

        [JsonPropertyName("garbage_collection")]
        public GarbageCollectionDetailsImpl GarbageCollectionDetails { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("channels")]
        public long TotalChannels { get; set; }

        [JsonPropertyName("send_pend")]
        public long SentPending { get; set; }

        [JsonPropertyName("global_prefetch_count")]
        public uint GlobalPrefetchCount { get; set; }

        [JsonPropertyName("prefetch_count")]
        public uint PrefetchCount { get; set; }

        [JsonPropertyName("acks_uncommitted")]
        public ulong UncommittedAcknowledgements { get; set; }

        [JsonPropertyName("messages_uncommitted")]
        public ulong UncommittedMessages { get; set; }

        [JsonPropertyName("messages_unconfirmed")]
        public ulong UnconfirmedMessages { get; set; }

        [JsonPropertyName("messages_unacknowledged")]
        public ulong UnacknowledgedMessages { get; set; }

        [JsonPropertyName("consumer_count")]
        public ulong TotalConsumers { get; set; }

        [JsonPropertyName("confirm")]
        public bool Confirm { get; set; }

        [JsonPropertyName("transactional")]
        public bool Transactional { get; set; }

        [JsonPropertyName("idle_since")]
        public DateTimeOffset IdleSince { get; set; }
        
        [JsonPropertyName("message_stats")]
        public ChannelOperationStatsImpl OperationStats { get; set; }
    }
}
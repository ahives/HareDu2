namespace HareDu.Model
{
    using System;
    using System.Text.Json.Serialization;

    public interface ChannelInfo
    {
        [JsonPropertyName("reductions_details")]
        Rate ReductionDetails { get; }

        [JsonPropertyName("reductions")]
        ulong TotalReductions { get; }

        [JsonPropertyName("vhost")]
        string VirtualHost { get; }

        [JsonPropertyName("node")]
        string Node { get; }

        [JsonPropertyName("user")]
        string User { get; }

        [JsonPropertyName("user_who_performed_action")]
        string UserWhoPerformedAction { get; }

        [JsonPropertyName("connected_at")]
        long ConnectedAt { get; }

        [JsonPropertyName("frame_max")]
        ulong FrameMax { get; }

        [JsonPropertyName("timeout")]
        long Timeout { get; }

        [JsonPropertyName("number")]
        ulong Number { get; }

        [JsonPropertyName("name")]
        string Name { get; }

        [JsonPropertyName("protocol")]
        string Protocol { get; }

        [JsonPropertyName("ssl_hash")]
        string SslHash { get; }

        [JsonPropertyName("ssl_cipher")]
        string SslCipher { get; }

        [JsonPropertyName("ssl_key_exchange")]
        string SslKeyExchange { get; }

        [JsonPropertyName("ssl_protocol")]
        string SslProtocol { get; }

        [JsonPropertyName("auth_mechanism")]
        string AuthenticationMechanism { get; }

        [JsonPropertyName("peer_cert_validity")]
        string PeerCertificateValidity { get; }

        [JsonPropertyName("peer_cert_issuer")]
        string PeerCertificateIssuer { get; }

        [JsonPropertyName("peer_cert_subject")]
        string PeerCertificateSubject { get; }

        [JsonPropertyName("ssl")]
        bool Ssl { get; }

        [JsonPropertyName("peer_host")]
        string PeerHost { get; }

        [JsonPropertyName("host")]
        string Host { get; }

        [JsonPropertyName("peer_port")]
        long PeerPort { get; }

        [JsonPropertyName("port")]
        long Port { get; }

        [JsonPropertyName("type")]
        string Type { get; }

        [JsonPropertyName("connection_details")]
        ConnectionDetails ConnectionDetails { get; }

        [JsonPropertyName("garbage_collection")]
        GarbageCollectionDetails GarbageCollectionDetails { get; }

        [JsonPropertyName("state")]
        string State { get; }

        [JsonPropertyName("channels")]
        long TotalChannels { get; }

        [JsonPropertyName("send_pend")]
        long SentPending { get; }

        [JsonPropertyName("global_prefetch_count")]
        uint GlobalPrefetchCount { get; }

        [JsonPropertyName("prefetch_count")]
        uint PrefetchCount { get; }

        [JsonPropertyName("acks_uncommitted")]
        ulong UncommittedAcknowledgements { get; }

        [JsonPropertyName("messages_uncommitted")]
        ulong UncommittedMessages { get; }

        [JsonPropertyName("messages_unconfirmed")]
        ulong UnconfirmedMessages { get; }

        [JsonPropertyName("messages_unacknowledged")]
        ulong UnacknowledgedMessages { get; }

        [JsonPropertyName("consumer_count")]
        ulong TotalConsumers { get; }

        [JsonPropertyName("confirm")]
        bool Confirm { get; }

        [JsonPropertyName("transactional")]
        bool Transactional { get; }

        [JsonPropertyName("idle_since")]
        DateTimeOffset IdleSince { get; }
        
        [JsonPropertyName("message_stats")]
        ChannelOperationStats OperationStats { get; }
    }
}
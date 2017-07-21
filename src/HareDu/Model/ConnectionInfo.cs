namespace HareDu.Model
{
    using Newtonsoft.Json;

    public interface ConnectionInfo
    {
        [JsonProperty("reductions_details")]
        Rate RateOfReduction { get; }

        [JsonProperty("reductions")]
        long TotalReductions { get; }

        [JsonProperty("recv_oct")]
        long ReceivedOctet { get; }

        [JsonProperty("recv_oct_details")]
        Rate RateOfReceviedOctet { get; }

        [JsonProperty("send_oct_details")]
        Rate RateOfSentOctet { get; }

        [JsonProperty("connected_at")]
        long ConnectedAt { get; }

        [JsonProperty("channel_max")]
        long ChannelMax { get; }

        [JsonProperty("frame_max")]
        long FrameMax { get; }

        [JsonProperty("timeout")]
        long Timeout { get; }

        [JsonProperty("vhost")]
        string VirtualHost { get; }

        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("channels")]
        long Channels { get; }

        [JsonProperty("send_pend")]
        long SendPending { get; }

        [JsonProperty("send_cnt")]
        long SendCount { get; }

        [JsonProperty("recv_cnt")]
        long ReceivedCount { get; }

        [JsonProperty("type")]
        string Type { get; }

        [JsonProperty("garbage_collection")]
        GarbageCollectionDetails GarbageCollectionDetails { get; }

        [JsonProperty("state")]
        string State { get; }

        [JsonProperty("ssl_hash")]
        string SslHash { get; }

        [JsonProperty("ssl_cipher")]
        string SslCipher { get; }

        [JsonProperty("ssl_key_exchange")]
        string SslKeyExchange { get; }

        [JsonProperty("ssl_protocol")]
        string SslProtocol { get; }

        [JsonProperty("auth_mechanism")]
        string AuthenticationMechanism { get; }

        [JsonProperty("peer_cert_validity")]
        string PeerCertificateValidity { get; }

        [JsonProperty("peer_cert_issuer")]
        string PeerCertificateIssuer { get; }

        [JsonProperty("peer_cert_subject")]
        string PeerCertificateSubject { get; }

        [JsonProperty("ssl")]
        bool Ssl { get; }

        [JsonProperty("peer_host")]
        string PeerHost { get; }

        [JsonProperty("host")]
        string Host { get; }

        [JsonProperty("peer_port")]
        long PeerPort { get; }

        [JsonProperty("port")]
        long Port { get; }
    }
}
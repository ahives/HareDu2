// Copyright 2013-2020 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Model
{
    using System;
    using Newtonsoft.Json;

    public interface ChannelInfo
    {
        [JsonProperty("reductions_details")]
        Rate ReductionDetails { get; }

        [JsonProperty("reductions")]
        ulong TotalReductions { get; }

        [JsonProperty("vhost")]
        string VirtualHost { get; }

        [JsonProperty("node")]
        string Node { get; }

        [JsonProperty("user")]
        string User { get; }

        [JsonProperty("user_who_performed_action")]
        string UserWhoPerformedAction { get; }

        [JsonProperty("connected_at")]
        long ConnectedAt { get; }

        [JsonProperty("frame_max")]
        ulong FrameMax { get; }

        [JsonProperty("timeout")]
        long Timeout { get; }

        [JsonProperty("number")]
        ulong Number { get; }

        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("protocol")]
        string Protocol { get; }

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

        [JsonProperty("type")]
        string Type { get; }

        [JsonProperty("connection_details")]
        ConnectionDetails ConnectionDetails { get; }

        [JsonProperty("garbage_collection")]
        GarbageCollectionDetails GarbageCollectionDetails { get; }

        [JsonProperty("state")]
        string State { get; }

        [JsonProperty("channels")]
        long TotalChannels { get; }

        [JsonProperty("send_pend")]
        long SentPending { get; }

        [JsonProperty("global_prefetch_count")]
        uint GlobalPrefetchCount { get; }

        [JsonProperty("prefetch_count")]
        uint PrefetchCount { get; }

        [JsonProperty("acks_uncommitted")]
        ulong UncommittedAcknowledgements { get; }

        [JsonProperty("messages_uncommitted")]
        ulong UncommittedMessages { get; }

        [JsonProperty("messages_unconfirmed")]
        ulong UnconfirmedMessages { get; }

        [JsonProperty("messages_unacknowledged")]
        ulong UnacknowledgedMessages { get; }

        [JsonProperty("consumer_count")]
        ulong TotalConsumers { get; }

        [JsonProperty("confirm")]
        bool Confirm { get; }

        [JsonProperty("transactional")]
        bool Transactional { get; }

        [JsonProperty("idle_since")]
        DateTimeOffset IdleSince { get; }
        
        [JsonProperty("message_stats")]
        ChannelOperationStats OperationStats { get; }
    }
}
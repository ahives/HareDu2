// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Core.Model
{
    using Newtonsoft.Json;

    public interface ConnectionInfo
    {
        [JsonProperty("reductions_details")]
        Rate RateOfReduction { get; }

        [JsonProperty("reductions")]
        long TotalReductions { get; }

        [JsonProperty("recv_oct")]
        long PacketBytesReceived { get; }

        [JsonProperty("recv_oct_details")]
        Rate RateOfPacketsReceivedInOctets { get; }

        [JsonProperty("send_oct_details")]
        Rate RateOfPacketsSentInOctets { get; }

        [JsonProperty("connected_at")]
        long ConnectedAt { get; }

        [JsonProperty("channel_max")]
        long MaxChannels { get; }

        [JsonProperty("frame_max")]
        long MaxFrameSizeInBytes { get; }

        [JsonProperty("timeout")]
        long ConnectionTimeout { get; }

        [JsonProperty("vhost")]
        string VirtualHost { get; }

        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("channels")]
        long Channels { get; }

        [JsonProperty("send_pend")]
        long SendPending { get; }

        [JsonProperty("send_cnt")]
        long PacketsSent { get; }

        [JsonProperty("send_oct")]
        long PacketBytesSent { get; }

        [JsonProperty("recv_cnt")]
        long PacketsReceived { get; }

        [JsonProperty("type")]
        string Type { get; }

        [JsonProperty("garbage_collection")]
        GarbageCollectionDetails GarbageCollectionDetails { get; }

        [JsonProperty("state")]
        string State { get; }

        [JsonProperty("ssl_hash")]
        string SslHashFunction { get; }

        [JsonProperty("ssl_cipher")]
        string SslCipherAlgorithm { get; }

        [JsonProperty("ssl_key_exchange")]
        string SslKeyExchangeAlgorithm { get; }

        [JsonProperty("ssl_protocol")]
        string SslProtocol { get; }

        [JsonProperty("auth_mechanism")]
        string AuthenticationMechanism { get; }

        [JsonProperty("peer_cert_validity")]
        string TimePeriodPeerCertificateValid { get; }

        [JsonProperty("peer_cert_issuer")]
        string PeerCertificateIssuer { get; }

        [JsonProperty("peer_cert_subject")]
        string PeerCertificateSubject { get; }

        [JsonProperty("ssl")]
        bool IsSecure { get; }

        [JsonProperty("peer_host")]
        string PeerHost { get; }

        [JsonProperty("host")]
        string Host { get; }

        [JsonProperty("peer_port")]
        long PeerPort { get; }

        [JsonProperty("port")]
        long Port { get; }

        [JsonProperty("node")]
        string Node { get; }
    }
}
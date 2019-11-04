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
namespace HareDu.Model
{
    using System;
    using Newtonsoft.Json;

    public interface ConnectionInfo
    {
        [JsonProperty("reductions_details")]
        Rate RateOfReduction { get; }
        
        [JsonProperty("protocol")]
        string Protocol { get; }

        [JsonProperty("reductions")]
        ulong TotalReductions { get; }

        [JsonProperty("recv_oct")]
        ulong PacketBytesReceived { get; }

        [JsonProperty("recv_oct_details")]
        Rate RateOfPacketBytesReceived { get; }

        [JsonProperty("send_oct_details")]
        Rate RateOfPacketBytesSent { get; }

        [JsonProperty("connected_at")]
        long ConnectedAt { get; }

        [JsonProperty("channel_max")]
        ulong OpenChannelsLimit { get; }

        [JsonProperty("frame_max")]
        ulong MaxFrameSizeInBytes { get; }

        [JsonProperty("timeout")]
        long ConnectionTimeout { get; }

        [JsonProperty("vhost")]
        string VirtualHost { get; }

        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("channels")]
        ulong Channels { get; }

        [JsonProperty("send_pend")]
        ulong SendPending { get; }

        [JsonProperty("send_cnt")]
        ulong PacketsSent { get; }

        [JsonProperty("send_oct")]
        ulong PacketBytesSent { get; }

        [JsonProperty("recv_cnt")]
        ulong PacketsReceived { get; }

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
        bool IsSsl { get; }

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

        [JsonProperty("user")]
        string User { get; }

        [JsonProperty("user_who_performed_action")]
        string UserWhoPerformedAction { get; }

        [JsonProperty("client_properties")]
        ClientProperties ClientProperties { get; }
    }

    public interface ClientProperties
    {
        [JsonProperty("assembly")]
        string Assembly { get; }

        [JsonProperty("assembly_version")]
        string AssemblyVersion { get; }

        [JsonProperty("capabilities")]
        Capabilities Capabilities { get; }

        [JsonProperty("client_api")]
        string ClientApi { get; }

        [JsonProperty("connected")]
        DateTimeOffset Connected { get; }

        [JsonProperty("connection_name")]
        string ConnectionName { get; }

        [JsonProperty("copyright")]
        string Copyright { get; }

        [JsonProperty("hostname")]
        string Host { get; }

        [JsonProperty("information")]
        string Information { get; }

        [JsonProperty("platform")]
        string Platform { get; }

        [JsonProperty("process_id")]
        string ProcessId { get; }

        [JsonProperty("process_name")]
        string ProcessName { get; }

        [JsonProperty("product")]
        string Product { get; }

        [JsonProperty("version")]
        string Version { get; }
    }

    public interface Capabilities
    {
        [JsonProperty("authentication_failure_close")]
        bool AuthenticationFailureNotificationEnabled { get; }

        [JsonProperty("basic.nack")]
        bool NegativeAcknowledgmentNotificationsEnabled { get; }

        [JsonProperty("connection.blocked")]
        bool ConnectionBlockedNotificationsEnabled { get; }

        [JsonProperty("consumer_cancel_notify")]
        bool ConsumerCancellationNotificationsEnabled { get; }

        [JsonProperty("exchange_exchange_bindings")]
        bool ExchangeBindingEnabled { get; }

        [JsonProperty("publisher_confirms")]
        bool PublisherConfirmsEnabled { get; }
    }
}
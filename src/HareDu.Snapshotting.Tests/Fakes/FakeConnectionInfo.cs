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
namespace HareDu.Snapshotting.Tests.Fakes
{
    using HareDu.Model;

    public class FakeConnectionInfo :
        ConnectionInfo
    {
        public FakeConnectionInfo(int number)
        {
            TotalReductions = 897274932;
            MaxChannels = 79243773;
            MaxFrameSizeInBytes = 627378937423;
            VirtualHost = "TestVirtualHost";
            Name = $"Connection {number}";
            Channels = 7687264882;
            SendPending = 686219897;
            PacketsSent = 871998847;
            PacketBytesSent = 83008482374;
            PacketsReceived = 68721979894793;
        }

        public Rate RateOfReduction { get; }
        public ulong TotalReductions { get; }
        public ulong PacketBytesReceived { get; }
        public Rate RateOfPacketBytesReceived { get; }
        public Rate RateOfPacketBytesSent { get; }
        public long ConnectedAt { get; }
        public ulong MaxChannels { get; }
        public ulong MaxFrameSizeInBytes { get; }
        public long ConnectionTimeout { get; }
        public string VirtualHost { get; }
        public string Name { get; }
        public ulong Channels { get; }
        public ulong SendPending { get; }
        public ulong PacketsSent { get; }
        public ulong PacketBytesSent { get; }
        public ulong PacketsReceived { get; }
        public string Type { get; }
        public GarbageCollectionDetails GarbageCollectionDetails { get; }
        public string State { get; }
        public string SslHashFunction { get; }
        public string SslCipherAlgorithm { get; }
        public string SslKeyExchangeAlgorithm { get; }
        public string SslProtocol { get; }
        public string AuthenticationMechanism { get; }
        public string TimePeriodPeerCertificateValid { get; }
        public string PeerCertificateIssuer { get; }
        public string PeerCertificateSubject { get; }
        public bool IsSecure { get; }
        public string PeerHost { get; }
        public string Host { get; }
        public long PeerPort { get; }
        public long Port { get; }
        public string Node { get; }
    }
}
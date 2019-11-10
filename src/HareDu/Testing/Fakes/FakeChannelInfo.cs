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
namespace HareDu.Testing.Fakes
{
    using System;
    using Model;

    public class FakeChannelInfo :
        ChannelInfo
    {
        public FakeChannelInfo(int channel, int node)
        {
            TotalReductions = 872634826473;
            VirtualHost = "TestVirtualHost";
            Node = $"Node {node}";
            FrameMax = 728349837;
            Name = $"Channel {channel}";
            TotalChannels = 87;
            SentPending = 89;
            PrefetchCount = 78;
            UncommittedAcknowledgements = 98237843;
            UncommittedMessages = 383902;
            UnconfirmedMessages = 82930;
            UnacknowledgedMessages = 7882003;
            TotalConsumers = 90;
            ConnectionDetails = new ConnectionDetailsImpl();
        }

        public Rate ReductionDetails { get; }
        public ulong TotalReductions { get; }
        public string VirtualHost { get; }
        public string Node { get; }
        public string User { get; }
        public string UserWhoPerformedAction { get; }
        public long ConnectedAt { get; }
        public ulong FrameMax { get; }
        public long Timeout { get; }
        public ulong Number { get; }
        public string Name { get; }
        public string Protocol { get; }
        public string SslHash { get; }
        public string SslCipher { get; }
        public string SslKeyExchange { get; }
        public string SslProtocol { get; }
        public string AuthenticationMechanism { get; }
        public string PeerCertificateValidity { get; }
        public string PeerCertificateIssuer { get; }
        public string PeerCertificateSubject { get; }
        public bool Ssl { get; }
        public string PeerHost { get; }
        public string Host { get; }
        public long PeerPort { get; }
        public long Port { get; }
        public string Type { get; }
        public ConnectionDetails ConnectionDetails { get; }
        public GarbageCollectionDetails GarbageCollectionDetails { get; }
        public string State { get; }
        public long TotalChannels { get; }
        public long SentPending { get; }
        public uint GlobalPrefetchCount { get; }
        public uint PrefetchCount { get; }
        public ulong UncommittedAcknowledgements { get; }
        public ulong UncommittedMessages { get; }
        public ulong UnconfirmedMessages { get; }
        public ulong UnacknowledgedMessages { get; }
        public ulong TotalConsumers { get; }
        public bool Confirm { get; }
        public bool Transactional { get; }
        public DateTimeOffset IdleSince { get; }
        public ChannelOperationStats OperationStats { get; }


        class ConnectionDetailsImpl :
            ConnectionDetails
        {
            public ConnectionDetailsImpl()
            {
                Name = "Connection 1";
            }

            public string PeerHost { get; }
            public long PeerPort { get; }
            public string Name { get; }
        }
    }
}
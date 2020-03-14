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
namespace HareDu.Diagnostics.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Snapshotting.Model;

    public class FakeBrokerConnectivitySnapshot3 :
        BrokerConnectivitySnapshot
    {
        public FakeBrokerConnectivitySnapshot3()
        {
            Connections = GetConnections().ToList();
            ChannelsClosed = new ChurnMetricsImpl(79, 5.5M);
            ChannelsCreated = new ChurnMetricsImpl(79, 5.5M);
            ConnectionsClosed = new ChurnMetricsImpl(79, 5.5M);
            ConnectionsCreated = new ChurnMetricsImpl(79, 5.5M);
        }

        IEnumerable<ConnectionSnapshot> GetConnections()
        {
            yield return new ConnectionSnapshotImpl("Connection (1)");
            yield return new ConnectionSnapshotImpl("Connection (2)");
            yield return new ConnectionSnapshotImpl("Connection (3)");
        }

        public Guid SnapshotIdentifier { get; }
        public DateTimeOffset Timestamp { get; }
        public string BrokerVersion { get; }
        public string ClusterName { get; }
        public ChurnMetrics ChannelsClosed { get; }
        public ChurnMetrics ChannelsCreated { get; }
        public ChurnMetrics ConnectionsClosed { get; }
        public ChurnMetrics ConnectionsCreated { get; }
        public IReadOnlyList<ConnectionSnapshot> Connections { get; }

        
        class ChurnMetricsImpl :
            ChurnMetrics
        {
            public ChurnMetricsImpl(ulong total, decimal rate)
            {
                Total = total;
                Rate = rate;
            }

            public ulong Total { get; }
            public decimal Rate { get; }
        }

        
        class ConnectionSnapshotImpl :
            ConnectionSnapshot
        {
            public ConnectionSnapshotImpl(string identifier)
            {
                Identifier = identifier;
                Channels = GetChannels(identifier).ToList();
            }

            IEnumerable<ChannelSnapshot> GetChannels(string identifier)
            {
                yield return new ChannelSnapshotImpl(32, 50, "Channel (1)", identifier);
                yield return new ChannelSnapshotImpl(3, 5, "Channel (2)", identifier);
                yield return new ChannelSnapshotImpl(23, 3, "Channel (3)", identifier);
                yield return new ChannelSnapshotImpl(6, 84, "Channel (4)", identifier);
                yield return new ChannelSnapshotImpl(9, 9, "Channel (5)", identifier);
                yield return new ChannelSnapshotImpl(9, 72, "Channel (6)", identifier);
                yield return new ChannelSnapshotImpl(23, 73, "Channel (7)", identifier);
                yield return new ChannelSnapshotImpl(42, 50, "Channel (8)", identifier);
                yield return new ChannelSnapshotImpl(21, 43, "Channel (9)", identifier);
                yield return new ChannelSnapshotImpl(32, 8, "Channel (10)", identifier);
            }

            public string Identifier { get; }
            public NetworkTrafficSnapshot NetworkTraffic { get; }
            public ulong OpenChannelsLimit { get; }
            public string NodeIdentifier { get; }
            public string VirtualHost { get; }
            public ConnectionState State { get; }
            public IReadOnlyList<ChannelSnapshot> Channels { get; }


            class ChannelSnapshotImpl :
                ChannelSnapshot
            {
                public ChannelSnapshotImpl(uint prefetchCount, ulong unacknowledgedMessages, string identifier, string connectionIdentifier)
                {
                    PrefetchCount = prefetchCount;
                    UnacknowledgedMessages = unacknowledgedMessages;
                    Identifier = identifier;
                    ConnectionIdentifier = connectionIdentifier;
                }

                public uint PrefetchCount { get; }
                public ulong UncommittedAcknowledgements { get; }
                public ulong UncommittedMessages { get; }
                public ulong UnconfirmedMessages { get; }
                public ulong UnacknowledgedMessages { get; }
                public ulong Consumers { get; }
                public string Identifier { get; }
                public string ConnectionIdentifier { get; }
                public string Node { get; }
                public QueueOperationMetrics QueueOperations { get; }
            }
        }
    }
}
namespace HareDu.Diagnostics.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Snapshotting.Model;

    public class FakeBrokerConnectivitySnapshot2 :
        BrokerConnectivitySnapshot
    {
        public FakeBrokerConnectivitySnapshot2(decimal connectionsCreatedRate, decimal connectionsClosedRate)
        {
            Connections = GetConnections().ToList();
            ConnectionsCreated = new ChurnMetricsImpl(100000, connectionsCreatedRate);
            ConnectionsClosed = new ChurnMetricsImpl(174000, connectionsClosedRate);
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

        IEnumerable<ConnectionSnapshot> GetConnections()
        {
            yield return new FakeConnectionSnapshot("Connection1", 6);
        }

        
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


        class FakeConnectionSnapshot :
            ConnectionSnapshot
        {
            public FakeConnectionSnapshot(string identifier, ulong channelLimit)
            {
                Identifier = identifier;
                OpenChannelsLimit = channelLimit;
                Channels = GetChannels().ToList();
            }

            IEnumerable<ChannelSnapshot> GetChannels()
            {
                yield return new FakeChannelSnapshot("Channel1", 4, 2, 5, 8, 6, 1);
                yield return new FakeChannelSnapshot("Channel2", 4, 2, 5, 8, 6, 1);
                yield return new FakeChannelSnapshot("Channel3", 4, 2, 5, 8, 6, 1);
                yield return new FakeChannelSnapshot("Channel4", 4, 2, 5, 8, 6, 1);
                yield return new FakeChannelSnapshot("Channel5", 4, 2, 5, 8, 6, 1);
                yield return new FakeChannelSnapshot("Channel6", 4, 2, 5, 8, 6, 1);
            }

                
            class FakeChannelSnapshot :
                ChannelSnapshot
            {
                public FakeChannelSnapshot(string name, uint prefetchCount, ulong uncommittedAcknowledgements,
                    ulong uncommittedMessages, ulong unconfirmedMessages, ulong unacknowledgedMessages, ulong consumers)
                {
                    PrefetchCount = prefetchCount;
                    UncommittedAcknowledgements = uncommittedAcknowledgements;
                    UncommittedMessages = uncommittedMessages;
                    UnconfirmedMessages = unconfirmedMessages;
                    UnacknowledgedMessages = unacknowledgedMessages;
                    Consumers = consumers;
                    Identifier = name;
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

            public string Identifier { get; }
            public NetworkTrafficSnapshot NetworkTraffic { get; }
            public ulong OpenChannelsLimit { get; }
            public string NodeIdentifier { get; }
            public string VirtualHost { get; }
            public BrokerConnectionState State { get; }
            public IReadOnlyList<ChannelSnapshot> Channels { get; }
        }
    }
}
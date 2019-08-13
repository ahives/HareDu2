namespace HareDu.Diagnostics.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Linq;
    using Snapshotting;
    using Snapshotting.Model;

    public class FakeConnectivitySnapshot3 :
        ConnectivitySnapshot
    {
        public FakeConnectivitySnapshot3()
        {
            Connections = GetConnections().ToList();
        }

        public ChurnMetrics ChannelsClosed { get; }
        public ChurnMetrics ChannelsCreated { get; }
        public ChurnMetrics ConnectionsClosed { get; }
        public ChurnMetrics ConnectionsCreated { get; }
        public IReadOnlyList<ConnectionSnapshot> Connections { get; }

        IEnumerable<ConnectionSnapshot> GetConnections()
        {
            yield return new FakeConnectionSnapshot("Connection1", 6);
        }

            
        class FakeConnectionSnapshot :
            ConnectionSnapshot
        {
            public FakeConnectionSnapshot(string identifier, long channelLimit)
            {
                Identifier = identifier;
                ChannelLimit = channelLimit;
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
                public FakeChannelSnapshot(string name, long prefetchCount, long uncommittedAcknowledgements,
                    long uncommittedMessages, long unconfirmedMessages, long unacknowledgedMessages, long consumers)
                {
                    PrefetchCount = prefetchCount;
                    UncommittedAcknowledgements = uncommittedAcknowledgements;
                    UncommittedMessages = uncommittedMessages;
                    UnconfirmedMessages = unconfirmedMessages;
                    UnacknowledgedMessages = unacknowledgedMessages;
                    Consumers = consumers;
                    Name = name;
                }

                public long PrefetchCount { get; }
                public long UncommittedAcknowledgements { get; }
                public long UncommittedMessages { get; }
                public long UnconfirmedMessages { get; }
                public long UnacknowledgedMessages { get; }
                public long Consumers { get; }
                public string Name { get; }
                public string Node { get; }
            }

            public string Identifier { get; }
            public NetworkTrafficSnapshot NetworkTraffic { get; }
            public long ChannelLimit { get; }
            public string Node { get; }
            public string VirtualHost { get; }
            public IReadOnlyList<ChannelSnapshot> Channels { get; }
        }
    }
}
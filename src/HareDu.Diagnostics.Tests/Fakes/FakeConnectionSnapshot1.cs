namespace HareDu.Diagnostics.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Snapshotting.Model;

    public class FakeConnectionSnapshot1 :
        ConnectionSnapshot
    {
        public FakeConnectionSnapshot1(int numOfChannels, ulong channelLimit)
        {
            Identifier = "Connection1";
            OpenChannelsLimit = channelLimit;
            Channels = GetChannels(numOfChannels).ToList();
        }

        public string Identifier { get; }
        public NetworkTrafficSnapshot NetworkTraffic { get; }
        public ulong OpenChannelsLimit { get; }
        public string NodeIdentifier { get; }
        public string VirtualHost { get; }
        public BrokerConnectionState State { get; }
        public IReadOnlyList<ChannelSnapshot> Channels { get; }

        IEnumerable<ChannelSnapshot> GetChannels(int numOfChannels)
        {
            for (int i = 0; i < numOfChannels; i++)
            {
                yield return new FakeChannelSnapshot($"Channel{i}", 4, 2, 5, 8, 6, 1);
            }
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
    }
}
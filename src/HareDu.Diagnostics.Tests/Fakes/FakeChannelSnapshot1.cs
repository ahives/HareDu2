namespace HareDu.Diagnostics.Tests.Fakes
{
    using Snapshotting.Model;

    public class FakeChannelSnapshot1 :
        ChannelSnapshot
    {
        public FakeChannelSnapshot1(string name, uint prefetchCount, ulong uncommittedAcknowledgements,
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
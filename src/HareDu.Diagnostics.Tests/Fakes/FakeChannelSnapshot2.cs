namespace HareDu.Diagnostics.Tests.Fakes
{
    using Snapshotting.Model;

    public class FakeChannelSnapshot2 :
        ChannelSnapshot
    {
        public FakeChannelSnapshot2(uint prefetchCount)
        {
            PrefetchCount = prefetchCount;
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
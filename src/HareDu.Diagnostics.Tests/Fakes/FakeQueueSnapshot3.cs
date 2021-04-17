namespace HareDu.Diagnostics.Tests.Fakes
{
    using System;
    using Snapshotting.Model;

    public class FakeQueueSnapshot3 :
        QueueSnapshot
    {
        public FakeQueueSnapshot3(decimal consumerUtilization)
        {
            ConsumerUtilization = consumerUtilization;
        }

        public string Identifier { get; }
        public string VirtualHost { get; }
        public string Node { get; }
        public QueueChurnMetrics Messages { get; }
        public QueueMemoryDetails Memory { get; }
        public QueueInternals Internals { get; }
        public ulong Consumers { get; }
        public decimal ConsumerUtilization { get; }
        public DateTimeOffset IdleSince { get; }
    }
}
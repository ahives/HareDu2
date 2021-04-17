namespace HareDu.Diagnostics.Tests.Fakes
{
    using System;
    using Snapshotting.Model;

    public class FakeQueueSnapshot5 :
        QueueSnapshot
    {
        public FakeQueueSnapshot5(ulong total)
        {
            Messages = new QueueChurnMetricsImpl(total);
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

        
        class QueueChurnMetricsImpl :
            QueueChurnMetrics
        {
            public QueueChurnMetricsImpl(ulong total)
            {
                Incoming = new QueueDepthImpl(total);
            }

            public QueueDepth Incoming { get; }
            public QueueDepth Unacknowledged { get; }
            public QueueDepth Ready { get; }
            public QueueDepth Gets { get; }
            public QueueDepth GetsWithoutAck { get; }
            public QueueDepth Delivered { get; }
            public QueueDepth DeliveredWithoutAck { get; }
            public QueueDepth DeliveredGets { get; }
            public QueueDepth Redelivered { get; }
            public QueueDepth Acknowledged { get; }
            public QueueDepth Aggregate { get; }

            
            class QueueDepthImpl :
                QueueDepth
            {
                public QueueDepthImpl(ulong total)
                {
                    Total = total;
                }

                public ulong Total { get; }
                public decimal Rate { get; }
            }
        }
    }
}
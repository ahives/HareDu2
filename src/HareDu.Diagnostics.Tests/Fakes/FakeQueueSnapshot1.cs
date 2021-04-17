namespace HareDu.Diagnostics.Tests.Fakes
{
    using System;
    using Snapshotting.Model;

    public class FakeQueueSnapshot1 :
        QueueSnapshot
    {
        public FakeQueueSnapshot1(ulong incomingTotal, decimal incomingRate, ulong acknowledgedTotal, decimal acknowledgedRate)
        {
            Messages = new QueueChurnMetricsImpl(incomingTotal, incomingRate, acknowledgedTotal, acknowledgedRate);
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
            public QueueChurnMetricsImpl(ulong incomingTotal, decimal incomingRate, ulong acknowledgedTotal, decimal acknowledgedRate)
            {
                Incoming = new QueueDepthImpl(incomingTotal, incomingRate);
                Acknowledged = new QueueDepthImpl(acknowledgedTotal, acknowledgedRate);
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
                public QueueDepthImpl(ulong total, decimal rate)
                {
                    Total = total;
                    Rate = rate;
                }

                public ulong Total { get; }
                public decimal Rate { get; }
            }
        }
    }
}
namespace HareDu.Diagnostics.Tests.Fakes
{
    using System.Collections.Generic;
    using Snapshotting.Model;

    public class FakeBrokerQueuesSnapshot2 :
        BrokerQueuesSnapshot
    {
        public FakeBrokerQueuesSnapshot2(ulong total)
        {
            Churn = new BrokerQueueChurnMetricsImpl(total);
        }

        public string ClusterName { get; }
        public BrokerQueueChurnMetrics Churn { get; }
        public IReadOnlyList<QueueSnapshot> Queues { get; }

        
        class BrokerQueueChurnMetricsImpl :
            BrokerQueueChurnMetrics
        {
            public BrokerQueueChurnMetricsImpl(ulong total)
            {
                NotRouted = new QueueDepthImpl(total);
            }

            public long Persisted { get; }
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
            public QueueDepth NotRouted { get; }
            public QueueDepth Broker { get; }

            
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
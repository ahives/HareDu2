// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Analytics.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Snapshotting.Model;

    public class FakeBrokerQueuesSnapshot :
        BrokerQueuesSnapshot
    {
        public FakeBrokerQueuesSnapshot()
        {
            ClusterIdentifier = "Cluster 1";
            Churn = new BrokerQueueChurnMetricsImpl();
            Queues = GetQueues().ToList();
        }

        class BrokerQueueChurnMetricsImpl :
            BrokerQueueChurnMetrics
        {
            public BrokerQueueChurnMetricsImpl()
            {
                NotRouted = new QueueDepthImpl(5);
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
        }

        IEnumerable<QueueSnapshot> GetQueues()
        {
            yield return new FakeQueueSnapshot1(17, 0);
            yield return new FakeQueueSnapshot1(0, 1);
            yield return new FakeQueueSnapshot1(20, 2);
            yield return new FakeQueueSnapshot1(0, 3);
            yield return new FakeQueueSnapshot1(200, 4);
            yield return new FakeQueueSnapshot1(0, 5);
            yield return new FakeQueueSnapshot1(0, 5);
            yield return new FakeQueueSnapshot1(0, 5);
        }

        public Guid SnapshotIdentifier { get; }
        public DateTimeOffset Timestamp { get; }
        public string ClusterIdentifier { get; }
        public BrokerQueueChurnMetrics Churn { get; }
        public IReadOnlyList<QueueSnapshot> Queues { get; }


        class FakeQueueSnapshot1 :
            QueueSnapshot
        {
            public FakeQueueSnapshot1(ulong total, int number)
            {
                Identifier = $"Queue{number}";
                Node = "Node0";
                Messages = new QueueChurnMetricsImpl(total);
                Memory = new QueueMemoryDetailsImpl(total);
            }

            class QueueMemoryDetailsImpl :
                QueueMemoryDetails
            {
                public QueueMemoryDetailsImpl(ulong total)
                {
                    PagedOut = new PagedOutImpl(total);
                }

                class PagedOutImpl : PagedOut
                {
                    public PagedOutImpl(ulong total)
                    {
                        Total = total;
                    }

                    public ulong Total { get; }
                    public ulong Bytes { get; }
                }

                public ulong Total { get; }
                public PagedOut PagedOut { get; }
                public RAM RAM { get; }
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
                    Acknowledged = new QueueDepthImpl(total * 2);
                    Redelivered = new QueueDepthImpl(total * 3);
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
            }
        }


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
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
namespace HareDu.Diagnostics.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Snapshotting.Model;

    public class FakeBrokerQueuesSnapshot3 :
        BrokerQueuesSnapshot
    {
        public FakeBrokerQueuesSnapshot3()
        {
            Queues = GetQueues().ToList();
        }

        public BrokerQueueChurnMetrics Churn { get; }
        public IReadOnlyList<QueueSnapshot> Queues { get; }

        IEnumerable<QueueSnapshot> GetQueues()
        {
            yield return new FakeQueueSnapshot("FakeQueue1",
                "FakeVirtualHost",
                "FakeNode",
                83,
                33,
                8929,
                62,
                93,
                2,
                1.57M,
                new DateTimeOffset(2019, 8, 20, 8, 0, 55, TimeSpan.Zero));
            yield return new FakeQueueSnapshot("FakeQueue2",
                "FakeVirtualHost",
                "FakeNode",
                74,
                0,
                82334,
                94,
                894,
                34,
                29.9M,
                new DateTimeOffset(2019, 8, 20, 8, 0, 55, TimeSpan.Zero));
            yield return new FakeQueueSnapshot("FakeQueue3",
                "FakeVirtualHost",
                "FakeNode",
                100,
                0,
                87432,
                39,
                74,
                0,
                0,
                new DateTimeOffset(2019, 8, 20, 8, 0, 55, TimeSpan.Zero));
            yield return new FakeQueueSnapshot("FakeQueue4",
                "FakeVirtualHost",
                "FakeNode",
                8349,
                8292,
                723894,
                3445,
                949,
                74,
                30.5M,
                new DateTimeOffset(2019, 8, 20, 8, 0, 55, TimeSpan.Zero));
            yield return new FakeQueueSnapshot("FakeQueue5",
                "FakeVirtualHost",
                "FakeNode",
                100,
                0,
                892389,
                84,
                23,
                93,
                84.0M,
                new DateTimeOffset(2019, 8, 20, 8, 0, 55, TimeSpan.Zero));
        }

        class FakeQueueSnapshot :
            QueueSnapshot
        {
            public FakeQueueSnapshot(string name,
                string virtualHost,
                string node,
                long target,
                long total,
                long bytes,
                long unacknowledged,
                long ready,
                long consumers,
                decimal consumerUtilization,
                DateTimeOffset idleSince)
            {
                Name = name;
                VirtualHost = virtualHost;
                Node = node;
                Consumers = consumers;
                ConsumerUtilization = consumerUtilization;
                IdleSince = idleSince;
                Memory = new FakeQueueMemory(target, total, bytes, unacknowledged, ready);
                Churn = new QueueChurnMetricsImpl();
            }

            
            class QueueChurnMetricsImpl :
                QueueChurnMetrics
            {
                public QueueChurnMetricsImpl()
                {
                    Incoming = new QueueDepthImpl(768578, 3845.5M);
                    Unacknowledged = new QueueDepthImpl(8293, 774.5M);
                    Ready = new QueueDepthImpl(8381, 3433.5M);
                    Gets = new QueueDepthImpl(934, 500.5M);
                    GetsWithoutAck = new QueueDepthImpl(0, 0M);
                    Delivered = new QueueDepthImpl(7339, 948.5M);
                    DeliveredWithoutAck = new QueueDepthImpl(34, 5.5M);
                    DeliveredGets = new QueueDepthImpl(0, 0M);
                    Redelivered = new QueueDepthImpl(768578, 3845.5M);
                    Acknowledged = new QueueDepthImpl(9238, 8934.5M);
                    Aggregate = new QueueDepthImpl(823847, 9847.5M);
                }

                
                class QueueDepthImpl :
                    QueueDepth
                {
                    public QueueDepthImpl(int total, decimal rate)
                    {
                        Total = total;
                        Rate = rate;
                    }

                    public long Total { get; }
                    public decimal Rate { get; }
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

            public string Name { get; }
            public string VirtualHost { get; }
            public string Node { get; }
            public QueueChurnMetrics Churn { get; }
            public QueueMemoryDetails Memory { get; }
            public QueueInternals Internals { get; }
            public long Consumers { get; }
            public decimal ConsumerUtilization { get; }
            public DateTimeOffset IdleSince { get; }

            
            class FakeQueueMemory :
                QueueMemoryDetails
            {
                public FakeQueueMemory(long target, long total, long bytes, long unacknowledged, long ready)
                {
                    RAM = new FakeRAM(target, total, bytes, unacknowledged, ready);
                }

                public long Total { get; }
                public RAM RAM { get; }
                

                class FakeRAM :
                    RAM
                {
                    public FakeRAM(long target, long total, long bytes, long unacknowledged, long ready)
                    {
                        Target = target;
                        Total = total;
                        Bytes = bytes;
                        Unacknowledged = unacknowledged;
                        Ready = ready;
                    }

                    public long Target { get; }
                    public long Total { get; }
                    public long Bytes { get; }
                    public long Unacknowledged { get; }
                    public long Ready { get; }
                }
            }
        }
    }
}
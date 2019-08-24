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
    using System.Collections.Generic;
    using System.Linq;
    using Snapshotting.Model;

    public class FakeBrokerQueuesSnapshot :
        BrokerQueuesSnapshot
    {
        public FakeBrokerQueuesSnapshot()
        {
            Queues = GetQueues().ToList();
        }

        IEnumerable<QueueSnapshot> GetQueues()
        {
            yield return new FakeQueueSnapshot("FakeQueue1", "FakeVirtualHost", "FakeNode", 100, 150, 20394, 5, 9);
            yield return new FakeQueueSnapshot("FakeQueue2", "FakeVirtualHost", "FakeNode", 50, 34, 82341, 95, 5);
            yield return new FakeQueueSnapshot("FakeQueue3", "FakeVirtualHost", "FakeNode", 69, 95, 8384, 82, 94);
            yield return new FakeQueueSnapshot("FakeQueue4", "FakeVirtualHost", "FakeNode", 94, 49, 23484, 74, 4);
//            yield return new FakeQueueSnapshot("FakeQueue5", "FakeVirtualHost", "FakeNode", 100, 833, 87432, 39, 74);
            yield return new FakeQueueSnapshot("FakeQueue5", "FakeVirtualHost", "FakeNode", 100, 0, 87432, 39, 74);
        }

        class FakeQueueSnapshot :
            QueueSnapshot
        {
            public FakeQueueSnapshot(string name, string virtualHost, string node, long target, long total, long bytes, long unacknowledged, long ready)
            {
                Name = name;
                VirtualHost = virtualHost;
                Node = node;
                Memory = new FakeQueueMemory(target, total, bytes, unacknowledged, ready);
            }

            class FakeQueueMemory : QueueMemoryDetails
            {
                public FakeQueueMemory(long target, long total, long bytes, long unacknowledged, long ready)
                {
                    RAM = new FakeRAM(target, total, bytes, unacknowledged, ready);
                }

                class FakeRAM : RAM
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

                public long Total { get; }
                public RAM RAM { get; }
            }

            public string Name { get; }
            public string VirtualHost { get; }
            public string Node { get; }
            public QueueChurnMetrics Churn { get; }
            public QueueMemoryDetails Memory { get; }
            public QueueInternals Internals { get; }
        }

        public BrokerQueueChurnMetrics Churn { get; }
        public IReadOnlyList<QueueSnapshot> Queues { get; }
    }
}
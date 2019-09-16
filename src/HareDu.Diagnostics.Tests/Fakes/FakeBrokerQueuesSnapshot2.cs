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
    using Snapshotting.Model;

    public class FakeBrokerQueuesSnapshot2 :
        BrokerQueuesSnapshot
    {
        public FakeBrokerQueuesSnapshot2(ulong total)
        {
            Churn = new BrokerQueueChurnMetricsImpl(total);
        }

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
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
        public string NodeIdentifier { get; }
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
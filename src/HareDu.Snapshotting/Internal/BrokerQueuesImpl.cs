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
namespace HareDu.Snapshotting.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Core;
    using Core.Extensions;
    using Core.Model;
    using Model;

    class BrokerQueuesImpl :
        BaseSnapshot<BrokerQueuesSnapshot>,
        BrokerQueues
    {
        readonly List<IDisposable> _observers;
        
        public BrokerQueuesImpl(IResourceFactory factory)
            : base(factory)
        {
            _observers = new List<IDisposable>();
        }

        public Result<BrokerQueuesSnapshot> Take(CancellationToken cancellationToken = default)
        {
            var cluster = _factory
                .Resource<Cluster>()
                .GetDetails(cancellationToken)
                .Select(x => x.Data);
            
            var queues = _factory
                .Resource<Queue>()
                .GetAll(cancellationToken)
                .Select(x => x.Data);
            
            BrokerQueuesSnapshot snapshot = new BrokerQueuesSnapshotImpl(cluster, queues);

            NotifyObservers(snapshot);
            
            return new SuccessfulResult<BrokerQueuesSnapshot>(snapshot, null);
        }

        public ComponentSnapshot<BrokerQueuesSnapshot> RegisterObserver(IObserver<SnapshotContext<BrokerQueuesSnapshot>> observer)
        {
            if (observer != null)
            {
                _observers.Add(Subscribe(observer));
            }

            return this;
        }

        public ComponentSnapshot<BrokerQueuesSnapshot> RegisterObservers(
            IReadOnlyList<IObserver<SnapshotContext<BrokerQueuesSnapshot>>> observers)
        {
            if (observers != null)
            {
                for (int i = 0; i < observers.Count; i++)
                {
                    _observers.Add(Subscribe(observers[i]));
                }
            }

            return this;
        }

        
        class BrokerQueuesSnapshotImpl :
            BrokerQueuesSnapshot
        {
            public BrokerQueuesSnapshotImpl(ClusterInfo cluster, IReadOnlyList<QueueInfo> queues)
            {
                Churn = new BrokerQueueChurnMetricsImpl(cluster.MessageStats, cluster.QueueStats);
                Queues = queues
                    .Select(x => new QueueSnapshotImpl(x))
                    .Cast<QueueSnapshot>()
                    .ToList();
            }

            public BrokerQueueChurnMetrics Churn { get; }
            public IReadOnlyList<QueueSnapshot> Queues { get; }

            
            class QueueSnapshotImpl :
                QueueSnapshot
            {
                public QueueSnapshotImpl(QueueInfo queue)
                {
                    Name = queue.Name;
                    VirtualHost = queue.VirtualHost;
                    Node = queue.Node;
                    Churn = new QueueChurnMetricsImpl(queue);
                    Memory = new QueueMemoryDetailsImpl(queue);
                }

                public string Name { get; }
                public string VirtualHost { get; }
                public string Node { get; }
                public QueueChurnMetrics Churn { get; }
                public QueueMemoryDetails Memory { get; }
                public QueueInternals Internals { get; }

                
                class QueueMemoryDetailsImpl :
                    QueueMemoryDetails
                {
                    public QueueMemoryDetailsImpl(QueueInfo queue)
                    {
                        Total = queue.Memory;
                        RAM = new RAMImpl(queue);
                    }

                    public long Total { get; }
                    public RAM RAM { get; }

                    
                    class RAMImpl :
                        RAM
                    {
                        public RAMImpl(QueueInfo queue)
                        {
                            Target = queue.BackingQueueStatus.TargetTotalMessagesInRAM.ToLong();
                            Total = queue.MessagesInRam;
                            Bytes = queue.MessageBytesInRam;
                            Unacknowledged = queue.UnacknowledgedMessagesInRam;
                            Ready = queue.MessagesReadyForDeliveryInRam;
                        }

                        public long Target { get; }
                        public long Total { get; }
                        public long Bytes { get; }
                        public long Unacknowledged { get; }
                        public long Ready { get; }
                    }
                }


                class QueueChurnMetricsImpl :
                    QueueChurnMetrics
                {
                    public QueueChurnMetricsImpl(QueueInfo queue)
                    {
                        Incoming = new QueueDepthImpl(queue.MessageStats?.TotalMessagesPublished ?? 0, queue.MessageStats?.MessagesPublishedDetails?.Rate ?? 0);
                        Gets = new QueueDepthImpl(queue.MessageStats?.TotalMessageGets ?? 0, queue.MessageStats?.MessageGetDetails?.Rate ?? 0);
                        GetsWithoutAck = new QueueDepthImpl(queue.MessageStats?.TotalMessageGetsWithoutAck ?? 0, queue.MessageStats?.MessageGetsWithoutAckDetails?.Rate ?? 0);
                        DeliveredGets = new QueueDepthImpl(queue.MessageStats?.TotalMessageDeliveryGets ?? 0, queue.MessageStats?.MessageDeliveryGetDetails?.Rate ?? 0);
                        Delivered = new QueueDepthImpl(queue.MessageStats?.TotalMessagesDelivered ?? 0, queue.MessageStats?.MessageDeliveryDetails?.Rate ?? 0);
                        DeliveredWithoutAck = new QueueDepthImpl(queue.MessageStats?.TotalMessageDeliveredWithoutAck ?? 0, queue.MessageStats?.MessagesDeliveredWithoutAckDetails?.Rate ?? 0);
                        Redelivered = new QueueDepthImpl(queue.MessageStats?.TotalMessagesRedelivered ?? 0, queue.MessageStats?.MessagesRedeliveredDetails?.Rate ?? 0);
                        Acknowledged = new QueueDepthImpl(queue.MessageStats?.TotalMessagesAcknowledged ?? 0, queue.MessageStats?.MessagesAcknowledgedDetails?.Rate ?? 0);
                        Aggregate = new QueueDepthImpl(queue.TotalMessages, queue.RateOfMessages?.Rate ?? 0);
                        Ready = new QueueDepthImpl(queue.ReadyMessages, queue.RateOfReadyMessages?.Rate ?? 0);
                        Unacknowledged = new QueueDepthImpl(queue.UnacknowledgedMessages, queue.RateOfUnacknowledgedMessages?.Rate ?? 0);
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


            class BrokerQueueChurnMetricsImpl :
                BrokerQueueChurnMetrics
            {
                public BrokerQueueChurnMetricsImpl(MessageStats messageStats, QueueStats queueStats)
                {
                    Incoming = new QueueDepthImpl(messageStats.TotalMessagesPublished, messageStats.MessagesPublishedDetails?.Rate ?? 0);
                    NotRouted = new QueueDepthImpl(messageStats.TotalUnroutableMessages, messageStats.UnroutableMessagesDetails?.Rate ?? 0);
                    Gets = new QueueDepthImpl(messageStats.TotalMessageGets, messageStats.MessageGetDetails?.Rate ?? 0);
                    GetsWithoutAck = new QueueDepthImpl(messageStats.TotalMessageGetsWithoutAck, messageStats.MessageGetsWithoutAckDetails?.Rate ?? 0);
                    DeliveredGets = new QueueDepthImpl(messageStats.TotalMessageDeliveryGets, messageStats.MessageDeliveryGetDetails?.Rate ?? 0);
                    Delivered = new QueueDepthImpl(messageStats.TotalMessagesDelivered, messageStats.MessageDeliveryDetails?.Rate ?? 0);
                    DeliveredWithoutAck = new QueueDepthImpl(messageStats.TotalMessageDeliveredWithoutAck, messageStats.MessagesDeliveredWithoutAckDetails?.Rate ?? 0);
                    Redelivered = new QueueDepthImpl(messageStats.TotalMessagesRedelivered, messageStats.MessagesRedeliveredDetails?.Rate ?? 0);
                    Acknowledged = new QueueDepthImpl(messageStats.TotalMessagesAcknowledged, messageStats.MessagesAcknowledgedDetails?.Rate ?? 0);
                    Broker = new QueueDepthImpl(queueStats.TotalMessages, queueStats.RateOfMessages?.Rate ?? 0);
                    Ready = new QueueDepthImpl(queueStats.MessagesReadyForDelivery, queueStats.RateOfMessagesReadyForDelivery?.Rate ?? 0);
                    Unacknowledged = new QueueDepthImpl(queueStats.UnacknowledgedDeliveredMessages, queueStats.RateOfUnacknowledgedDeliveredMessages?.Rate ?? 0);
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


            class QueueDepthImpl :
                QueueDepth
            {
                public QueueDepthImpl(long total, decimal rate)
                {
                    Total = total;
                    Rate = rate;
                }

                public long Total { get; }
                public decimal Rate { get; }
            }
        }
    }
}
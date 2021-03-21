namespace HareDu.Snapshotting.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Extensions;
    using HareDu.Extensions;
    using HareDu.Model;
    using HareDu.Registration;
    using MassTransit;
    using Model;

    class BrokerQueuesImpl :
        BaseSnapshotLens<BrokerQueuesSnapshot>,
        SnapshotLens<BrokerQueuesSnapshot>
    {
        readonly List<IDisposable> _observers;

        public SnapshotHistory<BrokerQueuesSnapshot> History => _timeline.Value;

        public BrokerQueuesImpl(IBrokerObjectFactory factory)
            : base(factory)
        {
            _observers = new List<IDisposable>();
        }

        public async Task<SnapshotResult<BrokerQueuesSnapshot>> TakeSnapshot(CancellationToken cancellationToken = default)
        {
            var cluster = await _factory
                .GetBrokerSystemOverview(cancellationToken)
                .ConfigureAwait(false);

            if (cluster.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve cluster information."));
                
                return new EmptySnapshotResult<BrokerQueuesSnapshot>();
            }

            var queues = _factory
                .Object<Queue>()
                .GetAll(cancellationToken)
                .GetResult();

            if (queues.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve queue information."));
                
                return new EmptySnapshotResult<BrokerQueuesSnapshot>();
            }
            
            BrokerQueuesSnapshot snapshot = new BrokerQueuesSnapshotImpl(
                cluster.Select(x => x.Data),
                queues.Select(x => x.Data));
            
            string identifier = NewId.Next().ToString();
            DateTimeOffset timestamp = DateTimeOffset.UtcNow;

            SaveSnapshot(identifier, snapshot, timestamp);
            NotifyObservers(identifier, snapshot, timestamp);

            return new SnapshotResultImpl(identifier, snapshot, timestamp);
        }

        public SnapshotLens<BrokerQueuesSnapshot> RegisterObserver(IObserver<SnapshotContext<BrokerQueuesSnapshot>> observer)
        {
            if (observer != null)
            {
                _observers.Add(Subscribe(observer));
            }

            return this;
        }

        public SnapshotLens<BrokerQueuesSnapshot> RegisterObservers(
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
            public BrokerQueuesSnapshotImpl(SystemOverviewInfo systemOverview, IReadOnlyList<QueueInfo> queues)
            {
                ClusterName = systemOverview.ClusterName;
                Churn = new BrokerQueueChurnMetricsImpl(systemOverview.MessageStats, systemOverview.QueueStats);
                Queues = queues
                    .Select(x => new QueueSnapshotImpl(x))
                    .Cast<QueueSnapshot>()
                    .ToList();
            }

            public string ClusterName { get; }
            public BrokerQueueChurnMetrics Churn { get; }
            public IReadOnlyList<QueueSnapshot> Queues { get; }

            
            class QueueSnapshotImpl :
                QueueSnapshot
            {
                public QueueSnapshotImpl(QueueInfo queue)
                {
                    Identifier = queue.Name;
                    VirtualHost = queue.VirtualHost;
                    Node = queue.Node;
                    Messages = new QueueChurnMetricsImpl(queue);
                    Memory = new QueueMemoryDetailsImpl(queue);
                    Consumers = queue.Consumers;
                    ConsumerUtilization = queue.ConsumerUtilization;
                    IdleSince = queue.IdleSince;
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


                class QueueMemoryDetailsImpl :
                    QueueMemoryDetails
                {
                    public QueueMemoryDetailsImpl(QueueInfo queue)
                    {
                        Total = queue.Memory;
                        RAM = new RAMImpl(queue);
                        PagedOut = new PagedOutImpl(queue.TotalMessagesPagedOut, queue.MessageBytesPagedOut);
                    }

                    public ulong Total { get; }
                    public PagedOut PagedOut { get; }
                    public RAM RAM { get; }

                    
                    class PagedOutImpl :
                        PagedOut
                    {
                        public PagedOutImpl(ulong total, ulong bytes)
                        {
                            Total = total;
                            Bytes = bytes;
                        }

                        public ulong Total { get; }
                        public ulong Bytes { get; }
                    }

                    
                    class RAMImpl :
                        RAM
                    {
                        public RAMImpl(QueueInfo queue)
                        {
                            Target = queue.BackingQueueStatus.IsNull() ? 0 : queue.BackingQueueStatus.TargetTotalMessagesInRAM.ToLong();
                            Total = queue.MessagesInRAM;
                            Bytes = queue.MessageBytesInRAM;
                            Unacknowledged = queue.UnacknowledgedMessagesInRAM;
                            Ready = queue.MessagesReadyForDeliveryInRAM;
                        }

                        public ulong Target { get; }
                        public ulong Total { get; }
                        public ulong Bytes { get; }
                        public ulong Unacknowledged { get; }
                        public ulong Ready { get; }
                    }
                }


                class QueueChurnMetricsImpl :
                    QueueChurnMetrics
                {
                    public QueueChurnMetricsImpl(QueueInfo queue)
                    {
                        Incoming = new QueueDepthImpl(queue.MessageStats?.TotalMessagesPublished ?? 0, queue.MessageStats?.MessagesPublishedDetails?.Value ?? 0);
                        Gets = new QueueDepthImpl(queue.MessageStats?.TotalMessageGets ?? 0, queue.MessageStats?.MessageGetDetails?.Value ?? 0);
                        GetsWithoutAck = new QueueDepthImpl(queue.MessageStats?.TotalMessageGetsWithoutAck ?? 0, queue.MessageStats?.MessageGetsWithoutAckDetails?.Value ?? 0);
                        DeliveredGets = new QueueDepthImpl(queue.MessageStats?.TotalMessageDeliveryGets ?? 0, queue.MessageStats?.MessageDeliveryGetDetails?.Value ?? 0);
                        Delivered = new QueueDepthImpl(queue.MessageStats?.TotalMessagesDelivered ?? 0, queue.MessageStats?.MessageDeliveryDetails?.Value ?? 0);
                        DeliveredWithoutAck = new QueueDepthImpl(queue.MessageStats?.TotalMessageDeliveredWithoutAck ?? 0, queue.MessageStats?.MessagesDeliveredWithoutAckDetails?.Value ?? 0);
                        Redelivered = new QueueDepthImpl(queue.MessageStats?.TotalMessagesRedelivered ?? 0, queue.MessageStats?.MessagesRedeliveredDetails?.Value ?? 0);
                        Acknowledged = new QueueDepthImpl(queue.MessageStats?.TotalMessagesAcknowledged ?? 0, queue.MessageStats?.MessagesAcknowledgedDetails?.Value ?? 0);
                        Aggregate = new QueueDepthImpl(queue.TotalMessages, queue.MessageDetails?.Value ?? 0);
                        Ready = new QueueDepthImpl(queue.ReadyMessages, queue.ReadyMessageDetails?.Value ?? 0);
                        Unacknowledged = new QueueDepthImpl(queue.UnacknowledgedMessages, queue.UnacknowledgedMessageDetails?.Value ?? 0);
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
                    Incoming = new QueueDepthImpl(messageStats.TotalMessagesPublished, messageStats.MessagesPublishedDetails?.Value ?? 0);
                    NotRouted = new QueueDepthImpl(messageStats.TotalUnroutableMessages, messageStats.UnroutableMessagesDetails?.Value ?? 0);
                    Gets = new QueueDepthImpl(messageStats.TotalMessageGets, messageStats.MessageGetDetails?.Value ?? 0);
                    GetsWithoutAck = new QueueDepthImpl(messageStats.TotalMessageGetsWithoutAck, messageStats.MessageGetsWithoutAckDetails?.Value ?? 0);
                    DeliveredGets = new QueueDepthImpl(messageStats.TotalMessageDeliveryGets, messageStats.MessageDeliveryGetDetails?.Value ?? 0);
                    Delivered = new QueueDepthImpl(messageStats.TotalMessagesDelivered, messageStats.MessageDeliveryDetails?.Value ?? 0);
                    DeliveredWithoutAck = new QueueDepthImpl(messageStats.TotalMessageDeliveredWithoutAck, messageStats.MessagesDeliveredWithoutAckDetails?.Value ?? 0);
                    Redelivered = new QueueDepthImpl(messageStats.TotalMessagesRedelivered, messageStats.MessagesRedeliveredDetails?.Value ?? 0);
                    Acknowledged = new QueueDepthImpl(messageStats.TotalMessagesAcknowledged, messageStats.MessagesAcknowledgedDetails?.Value ?? 0);
                    Broker = new QueueDepthImpl(queueStats.TotalMessages, queueStats.MessageDetails?.Value ?? 0);
                    Ready = new QueueDepthImpl(queueStats.TotalMessagesReadyForDelivery, queueStats.MessagesReadyForDeliveryDetails?.Value ?? 0);
                    Unacknowledged = new QueueDepthImpl(queueStats.TotalUnacknowledgedDeliveredMessages, queueStats.UnacknowledgedDeliveredMessagesDetails?.Value ?? 0);
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
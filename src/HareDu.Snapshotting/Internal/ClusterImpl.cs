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

    class ClusterImpl :
        BaseSnapshotLens<ClusterSnapshot>,
        SnapshotLens<ClusterSnapshot>
    {
        readonly List<IDisposable> _observers;

        public SnapshotHistory<ClusterSnapshot> History => _timeline.Value;

        public ClusterImpl(IBrokerObjectFactory factory)
            : base(factory)
        {
            _observers = new List<IDisposable>();
        }

        public async Task<SnapshotResult<ClusterSnapshot>> TakeSnapshot(CancellationToken cancellationToken = default)
        {
            var cluster = await _factory
                .GetBrokerSystemOverview(cancellationToken)
                .ConfigureAwait(false);

            if (cluster.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve cluster information."));
                
                return new EmptySnapshotResult<ClusterSnapshot>();
            }

            var nodes = await _factory
                .GetAllNodes(cancellationToken)
                .ConfigureAwait(false);

            if (nodes.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve node information."));
                
                return new EmptySnapshotResult<ClusterSnapshot>();
            }
            
            ClusterSnapshot snapshot = new ClusterSnapshotImpl(
                cluster.Select(x => x.Data),
                nodes.Select(x => x.Data));
            
            string identifier = NewId.Next().ToString();
            DateTimeOffset timestamp = DateTimeOffset.UtcNow;

            SaveSnapshot(identifier, snapshot, timestamp);
            NotifyObservers(identifier, snapshot, timestamp);

            return new SnapshotResultImpl(identifier, snapshot, timestamp);
        }

        public SnapshotLens<ClusterSnapshot> RegisterObserver(IObserver<SnapshotContext<ClusterSnapshot>> observer)
        {
            if (observer != null)
                _observers.Add(Subscribe(observer));

            return this;
        }

        public SnapshotLens<ClusterSnapshot> RegisterObservers(
            IReadOnlyList<IObserver<SnapshotContext<ClusterSnapshot>>> observers)
        {
            if (observers == null)
                return this;
            
            for (int i = 0; i < observers.Count; i++)
                _observers.Add(Subscribe(observers[i]));

            return this;
        }


        class ClusterSnapshotImpl :
            ClusterSnapshot
        {
            public ClusterSnapshotImpl(SystemOverviewInfo systemOverview, IReadOnlyList<NodeInfo> nodes)
            {
                ClusterName = systemOverview.ClusterName;
                BrokerVersion = systemOverview.RabbitMqVersion;
                Nodes = GetNodes(systemOverview, nodes);
            }

            IReadOnlyList<NodeSnapshot> GetNodes(SystemOverviewInfo systemOverview, IReadOnlyList<NodeInfo> nodes)
            {
                return nodes
                    .Select(x => new NodeSnapshotImpl(systemOverview, x))
                    .Cast<NodeSnapshot>()
                    .ToList();
            }

            public string BrokerVersion { get; }
            public string ClusterName { get; }
            public IReadOnlyList<NodeSnapshot> Nodes { get; }


            class NodeSnapshotImpl :
                NodeSnapshot
            {
                public NodeSnapshotImpl(SystemOverviewInfo systemOverview, NodeInfo node)
                {
                    Identifier = node.Name;
                    Uptime = node.Uptime;
                    ClusterIdentifier = systemOverview.ClusterName;
                    OS = new OperatingSystemSnapshotImpl(node);
                    Runtime = new BrokerRuntimeSnapshotImpl(systemOverview, node);
                    ContextSwitching = new ContextSwitchDetailsImpl(node);
                    Disk = new DiskSnapshotImpl(node);
                    NetworkPartitions = node.Partitions.ToList();
                    AvailableCoresDetected = node.AvailableCoresDetected;
                    Memory = new MemorySnapshotImpl(node);
                    IsRunning = node.IsRunning;
                    InterNodeHeartbeat = node.NetworkTickTime;
                }

                public OperatingSystemSnapshot OS { get; }
                public string RatesMode { get; }
                public long Uptime { get; }
                public long InterNodeHeartbeat { get; }
                public string Identifier { get; }
                public string ClusterIdentifier { get; }
                public string Type { get; }
                public bool IsRunning { get; }
                public ulong AvailableCoresDetected { get; }
                public IReadOnlyList<string> NetworkPartitions { get; }
                public DiskSnapshot Disk { get; }
                public BrokerRuntimeSnapshot Runtime { get; }
                public MemorySnapshot Memory { get; }
                public ContextSwitchingDetails ContextSwitching { get; }

                
                class MemorySnapshotImpl :
                    MemorySnapshot
                {
                    public MemorySnapshotImpl(NodeInfo node)
                    {
                        NodeIdentifier = node.Name;
                        Used = node.MemoryUsed;
                        UsageRate = node.MemoryUsageDetails?.Value ?? 0;
                        Limit = node.MemoryLimit;
                        AlarmInEffect = node.MemoryAlarm;
                    }

                    public string NodeIdentifier { get; }
                    public ulong Used { get; }
                    public decimal UsageRate { get; }
                    public ulong Limit { get; }
                    public bool AlarmInEffect { get; }
                }


                class DiskSnapshotImpl :
                    DiskSnapshot
                {
                    public DiskSnapshotImpl(NodeInfo node)
                    {
                        NodeIdentifier = node.Name;
                        Capacity = new DiskCapacityDetailsImpl(node);
                        Limit = node.FreeDiskLimit;
                        AlarmInEffect = node.FreeDiskAlarm;
                        IO = new IOImpl(node);
                    }

                    public string NodeIdentifier { get; }
                    public DiskCapacityDetails Capacity { get; }
                    public ulong Limit { get; }
                    public bool AlarmInEffect { get; }
                    public IO IO { get; }


                    class IOImpl :
                        IO
                    {
                        public IOImpl(NodeInfo node)
                        {
                            Reads = new DiskUsageDetailsImpl(node.TotalIOReads,
                                node.IOReadDetails?.Value ?? 0,
                                node.TotalIOBytesRead,
                                node.IOBytesReadDetails?.Value ?? 0,
                                node.AvgIOReadTime,
                                node.AvgIOReadTimeDetails?.Value ?? 0);
                            Writes = new DiskUsageDetailsImpl(node.TotalIOWrites,
                                node.IOWriteDetails?.Value ?? 0,
                                node.TotalIOBytesWritten,
                                node.IOBytesWrittenDetails?.Value ?? 0,
                                node.AvgTimePerIOWrite,
                                node.AvgTimePerIOWriteDetails?.Value ?? 0);
                            Seeks = new DiskUsageDetailsImpl(node.IOSeekCount,
                                node.IOSeeksDetails?.Value ?? 0,
                                0,
                                0,
                                node.AverageIOSeekTime,
                                node.AvgIOSeekTimeDetails?.Value ?? 0);
                            FileHandles = new FileHandlesImpl(node);
                        }

                        public DiskUsageDetails Reads { get; }
                        public DiskUsageDetails Writes { get; }
                        public DiskUsageDetails Seeks { get; }
                        public FileHandles FileHandles { get; }


                        class FileHandlesImpl :
                            FileHandles
                        {
                            public FileHandlesImpl(NodeInfo node)
                            {
                                Recycled = node.TotalIOReopened;
                                Rate = node.IOReopenedDetails?.Value ?? 0;
                            }

                            public ulong Recycled { get; }
                            public decimal Rate { get; }
                        }


                        class DiskUsageDetailsImpl :
                            DiskUsageDetails
                        {
                            public DiskUsageDetailsImpl(ulong total, decimal rate, ulong totalBytes, decimal bytesRate,
                                decimal avgWallTime, decimal avgWallTimeRate)
                            {
                                Total = total;
                                Rate = rate;
                                Bytes = new BytesImpl(totalBytes, bytesRate);
                                WallTime = new DiskOperationWallTimeImpl(avgWallTime, avgWallTimeRate);
                            }

                            public ulong Total { get; }
                            public decimal Rate { get; }
                            public Bytes Bytes { get; }
                            public DiskOperationWallTime WallTime { get; }


                            class DiskOperationWallTimeImpl :
                                DiskOperationWallTime
                            {
                                public DiskOperationWallTimeImpl(decimal avg, decimal rate)
                                {
                                    Average = avg;
                                    Rate = rate;
                                }

                                public decimal Average { get; }
                                public decimal Rate { get; }
                            }


                            class BytesImpl :
                                Bytes
                            {
                                public BytesImpl(ulong total, decimal rate)
                                {
                                    Total = total;
                                    Rate = rate;
                                }

                                public ulong Total { get; }
                                public decimal Rate { get; }
                            }
                        }
                    }


                    class DiskCapacityDetailsImpl :
                        DiskCapacityDetails
                    {
                        public DiskCapacityDetailsImpl(NodeInfo node)
                        {
                            Available = node.FreeDiskSpace;
                            Rate = node.FreeDiskSpaceDetails?.Value ?? 0;
                        }

                        public ulong Available { get; }
                        public decimal Rate { get; }
                    }
                }


                class ContextSwitchDetailsImpl :
                    ContextSwitchingDetails
                {
                    public ContextSwitchDetailsImpl(NodeInfo node)
                    {
                        Total = node.ContextSwitches;
                        Rate = node.ContextSwitchDetails?.Value ?? 0;
                    }

                    public ulong Total { get; }
                    public decimal Rate { get; }
                }


                class BrokerRuntimeSnapshotImpl :
                    BrokerRuntimeSnapshot
                {
                    public BrokerRuntimeSnapshotImpl(SystemOverviewInfo systemOverview, NodeInfo node)
                    {
                        ClusterIdentifier = systemOverview.ClusterName;
                        Identifier = node.Name;
                        Version = systemOverview.ErlangVersion;
                        Processes = new RuntimeProcessChurnMetricsImpl(node.TotalProcesses, node.ProcessesUsed, node.ProcessUsageDetails?.Value ?? 0);
                        Database = new RuntimeDatabaseImpl(node);
                        GC = new GarbageCollectionImpl(node);
                    }

                    public string Identifier { get; }
                    public string ClusterIdentifier { get; }
                    public string Version { get; }
                    public RuntimeProcessChurnMetrics Processes { get; }
                    public RuntimeDatabase Database { get; }
                    public GarbageCollection GC { get; }

                    
                    class GarbageCollectionImpl :
                        GarbageCollection
                    {
                        public GarbageCollectionImpl(NodeInfo node)
                        {
                            ChannelsClosed = new CollectedGarbageImpl(node.TotalChannelsClosed, node.ClosedChannelDetails?.Value ?? 0);
                            ConnectionsClosed = new CollectedGarbageImpl(node.TotalConnectionsClosed, node.ClosedConnectionDetails?.Value ?? 0);
                            QueuesDeleted = new CollectedGarbageImpl(node.TotalQueuesDeleted, node.DeletedQueueDetails?.Value ?? 0);
                            ReclaimedBytes = new CollectedGarbageImpl(node.BytesReclaimedByGarbageCollector, node.ReclaimedBytesFromGCDetails?.Value ?? 0);
                        }

                        class CollectedGarbageImpl :
                            CollectedGarbage
                        {
                            public CollectedGarbageImpl(ulong total, decimal rate)
                            {
                                Total = total;
                                Rate = rate;
                            }

                            public ulong Total { get; }
                            public decimal Rate { get; }
                        }

                        public CollectedGarbage ChannelsClosed { get; }
                        public CollectedGarbage ConnectionsClosed { get; }
                        public CollectedGarbage QueuesDeleted { get; }
                        public CollectedGarbage ReclaimedBytes { get; }
                    }


                    class RuntimeDatabaseImpl :
                        RuntimeDatabase
                    {
                        public RuntimeDatabaseImpl(NodeInfo node)
                        {
                            Transactions = new TransactionDetailsImpl(node);
                            Index = new IndexDetailsImpl(node);
                            Storage = new StorageDetailsImpl(node);
                        }

                        public TransactionDetails Transactions { get; }
                        public IndexDetails Index { get; }
                        public StorageDetails Storage { get; }


                        class StorageDetailsImpl :
                            StorageDetails
                        {
                            public StorageDetailsImpl(NodeInfo node)
                            {
                                Reads = new MessageStoreDetailsImpl(node.TotalMessageStoreReads,
                                    node.MessageStoreReadDetails?.Value ?? 0);
                                Writes = new MessageStoreDetailsImpl(node.TotalMessageStoreWrites,
                                    node.MessageStoreWriteDetails?.Value ?? 0);
                            }

                            public MessageStoreDetails Reads { get; }
                            public MessageStoreDetails Writes { get; }


                            class MessageStoreDetailsImpl :
                                MessageStoreDetails
                            {
                                public MessageStoreDetailsImpl(ulong total, decimal rate)
                                {
                                    Total = total;
                                    Rate = rate;
                                }

                                public ulong Total { get; }
                                public decimal Rate { get; }
                            }
                        }


                        class IndexDetailsImpl :
                            IndexDetails
                        {
                            public IndexDetailsImpl(NodeInfo node)
                            {
                                Reads = new IndexUsageDetailsImpl(node.TotalQueueIndexReads,
                                    node.QueueIndexReadDetails?.Value ?? 0);
                                Writes = new IndexUsageDetailsImpl(node.TotalQueueIndexWrites,
                                    node.QueueIndexWriteDetails?.Value ?? 0);
                                Journal = new JournalDetailsImpl(node);
                            }

                            public IndexUsageDetails Reads { get; }
                            public IndexUsageDetails Writes { get; }
                            public JournalDetails Journal { get; }


                            class JournalDetailsImpl :
                                JournalDetails
                            {
                                public JournalDetailsImpl(NodeInfo node)
                                {
                                    Writes = new IndexUsageDetailsImpl(node.TotalQueueIndexJournalWrites,
                                        node.QueueIndexJournalWriteDetails?.Value ?? 0);
                                }

                                public IndexUsageDetails Writes { get; }
                            }


                            class IndexUsageDetailsImpl :
                                IndexUsageDetails
                            {
                                public IndexUsageDetailsImpl(ulong total, decimal rate)
                                {
                                    Total = total;
                                    Rate = rate;
                                }

                                public ulong Total { get; }
                                public decimal Rate { get; }
                            }
                        }


                        class TransactionDetailsImpl :
                            TransactionDetails
                        {
                            public TransactionDetailsImpl(NodeInfo node)
                            {
                                RAM = new PersistenceDetailsImpl(node.TotalMnesiaRamTransactions,
                                    node.MnesiaRAMTransactionCountDetails?.Value ?? 0);
                                Disk = new PersistenceDetailsImpl(node.TotalMnesiaDiskTransactions,
                                    node.MnesiaDiskTransactionCountDetails?.Value ?? 0);
                            }

                            public PersistenceDetails RAM { get; }
                            public PersistenceDetails Disk { get; }


                            class PersistenceDetailsImpl :
                                PersistenceDetails
                            {
                                public PersistenceDetailsImpl(ulong total, decimal rate)
                                {
                                    Total = total;
                                    Rate = rate;
                                }

                                public ulong Total { get; }
                                public decimal Rate { get; }
                            }
                        }
                    }


                    class RuntimeProcessChurnMetricsImpl :
                        RuntimeProcessChurnMetrics
                    {
                        public RuntimeProcessChurnMetricsImpl(ulong limit, ulong used, decimal usageRate)
                        {
                            Limit = limit;
                            Used = used;
                            UsageRate = usageRate;
                        }

                        public ulong Limit { get; }
                        public ulong Used { get; }
                        public decimal UsageRate { get; }
                    }
                }


                class OperatingSystemSnapshotImpl :
                    OperatingSystemSnapshot
                {
                    public OperatingSystemSnapshotImpl(NodeInfo node)
                    {
                        NodeIdentifier = node.Name;
                        ProcessId = node.OperatingSystemProcessId;
                        FileDescriptors = new FileDescriptorChurnMetricsImpl(node);
                        SocketDescriptors = new SocketDescriptorChurnMetricsImpl(node);
                    }

                    public string NodeIdentifier { get; }
                    public string ProcessId { get; }
                    public FileDescriptorChurnMetrics FileDescriptors { get; }
                    public SocketDescriptorChurnMetrics SocketDescriptors { get; }


                    class SocketDescriptorChurnMetricsImpl :
                        SocketDescriptorChurnMetrics
                    {
                        public SocketDescriptorChurnMetricsImpl(NodeInfo node)
                        {
                            Available = node.TotalSocketsAvailable;
                            Used = node.SocketsUsed;
                            UsageRate = node.SocketsUsedDetails?.Value ?? 0;
                        }

                        public ulong Available { get; }
                        public ulong Used { get; }
                        public decimal UsageRate { get; }
                    }


                    class FileDescriptorChurnMetricsImpl :
                        FileDescriptorChurnMetrics
                    {
                        public FileDescriptorChurnMetricsImpl(NodeInfo node)
                        {
                            Available = node.TotalFileDescriptors;
                            Used = node.FileDescriptorUsed;
                            UsageRate = node.FileDescriptorUsedDetails?.Value ?? 0;
                            OpenAttempts = node.TotalOpenFileHandleAttempts;
                            OpenAttemptRate = node.FileHandleOpenAttemptDetails?.Value ?? 0;
                            AvgTimePerOpenAttempt = node.FileHandleOpenAttemptAvgTimeDetails?.Value ?? 0;
                            AvgTimeRatePerOpenAttempt = node.FileHandleOpenAttemptAvgTimeDetails?.Value ?? 0;
                        }

                        public ulong Available { get; }
                        public ulong Used { get; }
                        public decimal UsageRate { get; }
                        public ulong OpenAttempts { get; }
                        public decimal OpenAttemptRate { get; }
                        public decimal AvgTimePerOpenAttempt { get; }
                        public decimal AvgTimeRatePerOpenAttempt { get; }
                    }
                }
            }
        }
    }
}
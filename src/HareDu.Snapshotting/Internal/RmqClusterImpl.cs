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
    using Core.Extensions;
    using HareDu.Model;
    using Model;

    class RmqClusterImpl :
        BaseSnapshot<ClusterSnapshot>,
        RmqCluster
    {
        readonly List<IDisposable> _observers;

        public IReadOnlyList<SnapshotContext<ClusterSnapshot>> Snapshots => _snapshots;

        public RmqClusterImpl(IBrokerObjectFactory factory)
            : base(factory)
        {
            _observers = new List<IDisposable>();
        }

        public ResourceSnapshot<ClusterSnapshot> Execute(CancellationToken cancellationToken = default)
        {
            var cluster = _factory
                .Object<Cluster>()
                .GetDetails(cancellationToken)
                .Unfold();

            if (cluster.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve cluster information."));
                return this;
            }

            var nodes = _factory
                .Object<Node>()
                .GetAll(cancellationToken)
                .Unfold();

            if (nodes.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve node information."));
                return this;
            }
            
            ClusterSnapshot snapshot = new ClusterSnapshotImpl(cluster.Select(x => x.Data), nodes.Select(x => x.Data));
            SnapshotContext<ClusterSnapshot> context = new SnapshotContextImpl(snapshot);

            SaveSnapshot(context);
            NotifyObservers(context);

            return this;
        }

        public ResourceSnapshot<ClusterSnapshot> RegisterObserver(IObserver<SnapshotContext<ClusterSnapshot>> observer)
        {
            if (observer != null)
            {
                _observers.Add(Subscribe(observer));
            }

            return this;
        }

        public ResourceSnapshot<ClusterSnapshot> RegisterObservers(
            IReadOnlyList<IObserver<SnapshotContext<ClusterSnapshot>>> observers)
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


        class ClusterSnapshotImpl :
            ClusterSnapshot
        {
            public ClusterSnapshotImpl(ClusterInfo cluster, IReadOnlyList<NodeInfo> nodes)
            {
                ClusterName = cluster.ClusterName;
                BrokerVersion = cluster.RabbitMqVersion;
                Nodes = GetNodes(cluster, nodes);
            }

            IReadOnlyList<NodeSnapshot> GetNodes(ClusterInfo cluster, IReadOnlyList<NodeInfo> nodes)
            {
                return nodes
                    .Select(x => new NodeSnapshotImpl(cluster, x))
                    .Cast<NodeSnapshot>()
                    .ToList();
            }

            public string BrokerVersion { get; }
            public string ClusterName { get; }
            public IReadOnlyList<NodeSnapshot> Nodes { get; }


            class NodeSnapshotImpl :
                NodeSnapshot
            {
                public NodeSnapshotImpl(ClusterInfo cluster, NodeInfo node)
                {
                    Identifier = node.Name;
                    ClusterIdentifier = cluster.ClusterName;
                    OS = new OperatingSystemSnapshotImpl(node);
                    Runtime = new BrokerRuntimeSnapshotImpl(cluster, node);
                    IO = new IOImpl(cluster.MessageStats, node);
                    ContextSwitching = new ContextSwitchDetailsImpl(node);
                    Disk = new DiskSnapshotImpl(node);
                    NetworkPartitions = node.Partitions;
                    AvailableCoresDetected = node.AvailableCoresDetected;
                    Memory = new MemorySnapshotImpl(node);
                    RuntimeDatabase = new RuntimeDatabaseImpl(node);
                    IsRunning = node.IsRunning;
                }

                public OperatingSystemSnapshot OS { get; }
                public string RatesMode { get; }
                public long Uptime { get; }
                public int RunQueue { get; }
                public long InterNodeHeartbeat { get; }
                public string Identifier { get; }
                public string ClusterIdentifier { get; }
                public string Type { get; }
                public bool IsRunning { get; }
                public ulong AvailableCoresDetected { get; }
                public IList<string> NetworkPartitions { get; }
                public DiskSnapshot Disk { get; }
                public IO IO { get; }
                public BrokerRuntimeSnapshot Runtime { get; }
                public RuntimeDatabase RuntimeDatabase { get; }
                public MemorySnapshot Memory { get; }
                public GarbageCollection GC { get; }
                public ContextSwitchingDetails ContextSwitching { get; }

                
                class RuntimeDatabaseImpl :
                    RuntimeDatabase
                {
                    public RuntimeDatabaseImpl(NodeInfo node)
                    {
                        Transactions = new TransactionDetailsImpl(node);
                        Index = new IndexDetailsImpl(node);
                    }

                    public TransactionDetails Transactions { get; }
                    public IndexDetails Index { get; }
                    public StorageDetails Storage { get; }

                    
                    class IndexDetailsImpl :
                        IndexDetails
                    {
                        public IndexDetailsImpl(NodeInfo node)
                        {
                            Reads = new IndexUsageDetailsImpl(node.TotalQueueIndexReads, node.QueueIndexReadCountDetails?.Rate ?? 0);
                            Writes = new IndexUsageDetailsImpl(node.TotalQueueIndexWrites, node.QueueIndexWriteCountDetails?.Rate ?? 0);
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
                                Writes = new IndexUsageDetailsImpl(node.TotalQueueIndexJournalWrites, node.QueueIndexJournalWriteCountDetails?.Rate ?? 0);
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
                            RAM = new PersistenceDetailsImpl(node.TotalMnesiaRamTransactions, node.MnesiaRAMTransactionCountDetails?.Rate ?? 0);
                            Disk = new PersistenceDetailsImpl(node.TotalMnesiaDiskTransactions, node.MnesiaDiskTransactionCountDetails?.Rate ?? 0);
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

                
                class MemorySnapshotImpl :
                    MemorySnapshot
                {
                    public MemorySnapshotImpl(NodeInfo node)
                    {
                        NodeIdentifier = node.Name;
                        Used = node.MemoryUsed;
                        UsageRate = node.MemoryUsageDetails?.Rate ?? 0;
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
                    }

                    public string NodeIdentifier { get; }
                    public DiskCapacityDetails Capacity { get; }
                    public ulong Limit { get; }
                    public bool AlarmInEffect { get; }


                    class DiskCapacityDetailsImpl :
                        DiskCapacityDetails
                    {
                        public DiskCapacityDetailsImpl(NodeInfo node)
                        {
                            Available = node.FreeDiskSpace;
                            Rate = node.FreeDiskSpaceDetails?.Rate ?? 0;
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
                        Rate = node.ContextSwitchDetails?.Rate ?? 0;
                    }

                    public long Total { get; }
                    public decimal Rate { get; }
                }


                class BrokerRuntimeSnapshotImpl :
                    BrokerRuntimeSnapshot
                {
                    public BrokerRuntimeSnapshotImpl(ClusterInfo cluster, NodeInfo node)
                    {
                        ClusterIdentifier = cluster.ClusterName;
                        Identifier = node.Name;
                        Version = cluster.ErlangVersion;
                        Processes = new RuntimeProcessChurnMetricsImpl(node.TotalProcesses, node.ProcessesUsed, node.ProcessUsageDetails?.Rate ?? 0);
                    }

                    public string Identifier { get; }
                    public string ClusterIdentifier { get; }
                    public string Version { get; }
                    public RuntimeProcessChurnMetrics Processes { get; }


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
                            UsageRate = node.SocketsUsedDetails?.Rate ?? 0;
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
                            UsageRate = node.FileDescriptorUsedDetails?.Rate ?? 0;
                            OpenAttempts = node.TotalOpenFileHandleAttempts;
                            OpenAttemptRate = node.FileHandleOpenAttemptCountDetails?.Rate ?? 0;
                            AvgTimePerOpenAttempt = node.FileHandleOpenAttemptAvgTimeDetails?.Rate ?? 0;
                            AvgTimeRatePerOpenAttempt = node.FileHandleOpenAttemptAvgTimeDetails?.Rate ?? 0;
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


            class IOImpl :
                IO
            {
                public IOImpl(MessageStats stats, NodeInfo node)
                {
                    Reads = new DiskUsageDetailsImpl(stats.TotalDiskReads,
                        stats.DiskReadDetails?.Rate ?? 0,
                        node.TotalIOBytesRead,
                        node.IOReadBytesDetails?.Rate ?? 0,
                        node.AvgIOReadTime,
                        node.AvgIOReadTimeDetails?.Rate ?? 0);
                    Writes = new DiskUsageDetailsImpl(stats.TotalDiskWrites,
                        stats.DiskWriteDetails?.Rate ?? 0,
                        node.TotalIOWriteBytes,
                        node.IOWriteBytesDetail?.Rate ?? 0,
                        node.AvgTimePerIOWrite,
                        node.AvgTimePerIOWriteDetails?.Rate ?? 0);
                    Seeks = new DiskUsageDetailsImpl(node.IOSeekCount,
                        node.RateOfIOSeeks?.Rate ?? 0,
                        0,
                        0,
                        node.AverageIOSeekTime,
                        node.AvgIOSeekTimeDetails?.Rate ?? 0);
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
                        Recycled = node.IOReopenCount;
                        Rate = node.RateOfIOReopened?.Rate ?? 0;
                    }

                    public ulong Recycled { get; }
                    public decimal Rate { get; }
                }


                class DiskUsageDetailsImpl :
                    DiskUsageDetails
                {
                    public DiskUsageDetailsImpl(ulong total, decimal rate, ulong totalBytes, decimal bytesRate,
                        ulong avgWallTime, decimal avgWallTimeRate)
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
                        public DiskOperationWallTimeImpl(ulong avg, decimal rate)
                        {
                            Average = avg;
                            Rate = rate;
                        }

                        public ulong Average { get; }
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
        }
    }
}
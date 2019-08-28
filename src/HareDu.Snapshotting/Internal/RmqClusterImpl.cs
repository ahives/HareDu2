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

    class RmqClusterImpl :
        BaseSnapshot<ClusterSnapshot>,
        RmqCluster
    {
        readonly List<IDisposable> _observers;
        readonly List<Result<ClusterSnapshot>> _snapshots;

        public IReadOnlyList<Result<ClusterSnapshot>> Snapshots => _snapshots;

        public RmqClusterImpl(IResourceFactory factory)
            : base(factory)
        {
            _observers = new List<IDisposable>();
            _snapshots = new List<Result<ClusterSnapshot>>();
        }

        public ResourceSnapshot<ClusterSnapshot> TakeSnapshot(CancellationToken cancellationToken = default)
        {
            var cluster = _factory
                .Resource<Cluster>()
                .GetDetails(cancellationToken)
                .Select(x => x.Data);

            var nodes = _factory
                .Resource<Node>()
                .GetAll(cancellationToken)
                .Select(x => x.Data);
            
            ClusterSnapshot data = new ClusterSnapshotImpl(cluster, nodes);

            NotifyObservers(data);
            
            var snapshot = new SuccessfulResult<ClusterSnapshot>(data, null);

            _snapshots.Add(snapshot);

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
                RabbitMqVersion = cluster.RabbitMqVersion;
                Nodes = GetNodes(cluster, nodes);
                Timestamp = DateTimeOffset.Now;
            }

            IReadOnlyList<NodeSnapshot> GetNodes(ClusterInfo cluster, IReadOnlyList<NodeInfo> nodes)
            {
                return nodes
                    .Select(x => new NodeSnapshotImpl(cluster, x))
                    .Cast<NodeSnapshot>()
                    .ToList();
            }

            public string RabbitMqVersion { get; }
            public string ClusterName { get; }
            public IReadOnlyList<NodeSnapshot> Nodes { get; }
            public DateTimeOffset Timestamp { get; }


            class NodeSnapshotImpl :
                NodeSnapshot
            {
                public NodeSnapshotImpl(ClusterInfo cluster, NodeInfo node)
                {
                    OS = new OperatingSystemMetricsImpl(node);
                    Erlang = new ErlangMetricsImpl(cluster, node);
                    IO = new IOImpl(cluster.MessageStats, node);
                    ContextSwitching = new ContextSwitchDetailsImpl(node);
                }

                public OperatingSystemMetrics OS { get; }
                public string RatesMode { get; }
                public long Uptime { get; }
                public int RunQueue { get; }
                public long InterNodeHeartbeat { get; }
                public string Name { get; }
                public string Type { get; }
                public bool IsRunning { get; }
                public IO IO { get; }
                public ErlangMetrics Erlang { get; }
                public Mnesia Mnesia { get; }
                public NodeMemoryDetails NodeMemory { get; }
                public GarbageCollection GC { get; }
                public ContextSwitchingDetails ContextSwitching { get; }


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


                class ErlangMetricsImpl :
                    ErlangMetrics
                {
                    public ErlangMetricsImpl(ClusterInfo cluster, NodeInfo node)
                    {
                        Version = cluster.ErlangVerion;
                        MemoryUsed = node.MemoryUsed;
                        AvailableCores = node.Processors;
                        Processes = new ErlangProcessMetricsImpl(node.TotalProcesses, node.ProcessesUsed, node.ProcessUsageDetails?.Rate ?? 0);
                    }


                    class ErlangProcessMetricsImpl :
                        ErlangProcessMetrics
                    {
                        public ErlangProcessMetricsImpl(long limit, long used, decimal usageRate)
                        {
                            Limit = limit;
                            Used = used;
                            UsageRate = usageRate;
                        }

                        public long Limit { get; }
                        public long Used { get; }
                        public decimal UsageRate { get; }
                    }

                    public string Version { get; }
                    public long MemoryUsed { get; }
                    public long AvailableCores { get; }
                    public ErlangProcessMetrics Processes { get; }
                }


                class OperatingSystemMetricsImpl :
                    OperatingSystemMetrics
                {
                    public OperatingSystemMetricsImpl(NodeInfo node)
                    {
                        ProcessId = node.OperatingSystemProcessId;
                        FileDescriptors = new FileDescriptorMetricsImpl(node);
                        Sockets = new SocketMetricsImpl(node);
                    }


                    class SocketMetricsImpl :
                        SocketMetrics
                    {
                        public SocketMetricsImpl(NodeInfo node)
                        {
                            Available = node.TotalSocketsAvailable;
                            Used = node.SocketsUsed;
                            UsageRate = node.SocketsUsedDetails?.Rate ?? 0;
                        }

                        public long Available { get; }
                        public long Used { get; }
                        public decimal UsageRate { get; }
                    }


                    class FileDescriptorMetricsImpl :
                        FileDescriptorMetrics
                    {
                        public FileDescriptorMetricsImpl(NodeInfo node)
                        {
                            Available = node.TotalFileDescriptors;
                            Used = node.FileDescriptorUsed;
                            UsageRate = node.FileDescriptorUsedDetails?.Rate ?? 0;
                            OpenAttempts = node.TotalOpenFileHandleAttempts;
                            OpenAttemptRate = node.FileHandleOpenAttemptCountDetails?.Rate ?? 0;
                            OpenAttemptAvgTime = node.FileHandleOpenAttemptAvgTimeDetails?.Rate ?? 0;
                            OpenAttemptAvgTimeRate = node.FileHandleOpenAttemptAvgTimeDetails?.Rate ?? 0;
                        }

                        public long Available { get; }
                        public long Used { get; }
                        public decimal UsageRate { get; }
                        public long OpenAttempts { get; }
                        public decimal OpenAttemptRate { get; }
                        public decimal OpenAttemptAvgTime { get; }
                        public decimal OpenAttemptAvgTimeRate { get; }
                    }

                    public string ProcessId { get; }
                    public FileDescriptorMetrics FileDescriptors { get; }
                    public SocketMetrics Sockets { get; }
                }
            }


            class IOImpl :
                IO
            {
                public IOImpl(MessageStats stats, NodeInfo node)
                {
                    Disk = new DiskDetailsImpl(node);
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

                public DiskDetails Disk { get; }
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

                    public long Recycled { get; }
                    public decimal Rate { get; }
                }


                class DiskDetailsImpl :
                    DiskDetails
                {
                    public DiskDetailsImpl(NodeInfo node)
                    {
                        Capacity = new DiskCapacityDetailsImpl(node);
                        FreeLimit = node.FreeDiskLimit;
                        FreeAlarm = node.FreeDiskAlarm;
                    }

                    public DiskCapacityDetails Capacity { get; }
                    public string FreeLimit { get; }
                    public bool FreeAlarm { get; }


                    class DiskCapacityDetailsImpl :
                        DiskCapacityDetails
                    {
                        public DiskCapacityDetailsImpl(NodeInfo node)
                        {
                            Available = node.FreeDiskSpace;
                            Rate = node.FreeDiskSpaceDetails?.Rate ?? 0;
                        }

                        public long Available { get; }
                        public decimal Rate { get; }
                    }
                }


                class DiskUsageDetailsImpl :
                    DiskUsageDetails
                {
                    public DiskUsageDetailsImpl(long total, decimal rate, long totalBytes, decimal bytesRate,
                        long avgWallTime, decimal avgWallTimeRate)
                    {
                        Total = total;
                        Rate = rate;
                        Bytes = new BytesImpl(totalBytes, bytesRate);
                        WallTime = new DiskOperationWallTimeImpl(avgWallTime, avgWallTimeRate);
                    }

                    public long Total { get; }
                    public decimal Rate { get; }
                    public Bytes Bytes { get; }
                    public DiskOperationWallTime WallTime { get; }


                    class DiskOperationWallTimeImpl :
                        DiskOperationWallTime
                    {
                        public DiskOperationWallTimeImpl(long avg, decimal rate)
                        {
                            Average = avg;
                            Rate = rate;
                        }

                        public long Average { get; }
                        public decimal Rate { get; }
                    }


                    class BytesImpl :
                        Bytes
                    {
                        public BytesImpl(long total, decimal rate)
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
    }
}
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
namespace HareDu.Model
{
    using System.Collections.Generic;
    using Core.Extensions;
    using Core.Model;

    class ClusterStatusImpl :
        ClusterStatus
    {
        public ClusterStatusImpl(ClusterInfo cluster, IReadOnlyList<NodeInfo> nodes, IReadOnlyList<ConnectionInfo> connections, IReadOnlyList<ChannelInfo> channels)
        {
            ClusterName = cluster.ClusterName;
//            ErlangVerion = cluster.ErlangVerion;
            RabbitMqVersion = cluster.RabbitMqVersion;
            Queue = new QueueDetailsImpl(cluster.MessageStats);

            Nodes = GetNodes(cluster, nodes);
        }

        IReadOnlyList<NodeStatus> GetNodes(ClusterInfo cluster, IReadOnlyList<NodeInfo> nodes)
        {
            var nodeCluster = new List<NodeStatus>();
            for (int i = 0; i < nodes.Count; i++)
            {
                nodeCluster.Add(new NodeStatusImpl(cluster, nodes[i]));
            }

            return nodeCluster;
        }

        class NodeStatusImpl : NodeStatus
        {
            public NodeStatusImpl(ClusterInfo cluster, NodeInfo node)
            {
                OS = new OperatingSystemDetailsImpl(node);
                Erlang = new ErlangDetailsImpl(cluster, node);
                IO = new IOImpl(cluster.MessageStats, node);
                ContextSwitching = new ContextSwitchDetailsImpl(node);
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

            class ErlangDetailsImpl : ErlangDetails
            {
                public ErlangDetailsImpl(ClusterInfo cluster, NodeInfo node)
                {
                    Version = cluster.ErlangVerion;
                    MemoryUsed = node.MemoryUsed;
                    AvailableCPUCores = node.Processors;
                }

                public string Version { get; }
                public long MemoryUsed { get; }
                public long AvailableCPUCores { get; }
                public MemoryUsageDetails MemoryUsageDetails { get; }
                public ErlangProcessDetails Processes { get; }
            }


            class OperatingSystemDetailsImpl :
                OperatingSystemDetails
            {
                public OperatingSystemDetailsImpl(NodeInfo node)
                {
                    ProcessId = node.OperatingSystemProcessId;
                    FileDescriptors = new FileDescriptorDetailsImpl(node);
                    Sockets = new SocketDetailsImpl(node);
                }

                
                class SocketDetailsImpl :
                    SocketDetails
                {
                    public SocketDetailsImpl(NodeInfo node)
                    {
                        Available = node.TotalSocketsAvailable;
                        Used = node.SocketsUsed;
                        UsageRate = node.SocketsUsedDetails.Rate;
                    }

                    public long Available { get; }
                    public long Used { get; }
                    public decimal UsageRate { get; }
                }


                class FileDescriptorDetailsImpl :
                    FileDescriptorDetails
                {
                    public FileDescriptorDetailsImpl(NodeInfo node)
                    {
                        Available = node.TotalFileDescriptors;
                        Used = node.FileDescriptorUsed;
                        UsageRate = node.FileDescriptorUsedDetails.Rate;
                        FileDescriptorOpenAttempts = new FileDescriptorOpenAttemptsImpl(node);
                    }

                    public long Available { get; }
                    public long Used { get; }
                    public decimal UsageRate { get; }
                    public FileDescriptorOpenAttempts FileDescriptorOpenAttempts { get; }

                    
                    class FileDescriptorOpenAttemptsImpl :
                        FileDescriptorOpenAttempts
                    {
                        public FileDescriptorOpenAttemptsImpl(NodeInfo node)
                        {
                            OpenAttempts = node.TotalOpenFileHandleAttempts;
                            OpenAttemptRate = node.FileHandleOpenAttemptCountDetails.Rate;
                            OpenAttemptAvgTime = node.FileHandleOpenAttemptAvgTimeDetails.Rate;
                            FileHandleOpenAttemptAvgTimeRate = node.FileHandleOpenAttemptAvgTimeDetails.Rate;
                        }

                        public long OpenAttempts { get; }
                        public decimal OpenAttemptRate { get; }
                        public decimal OpenAttemptAvgTime { get; }
                        public decimal FileHandleOpenAttemptAvgTimeRate { get; }
                    }
                }

                public string ProcessId { get; }
                public FileDescriptorDetails FileDescriptors { get; }
                public SocketDetails Sockets { get; }
            }

            public OperatingSystemDetails OS { get; }
            public string RatesMode { get; }
            public long Uptime { get; }
            public int RunQueue { get; }
            public long InterNodeHeartbeat { get; }
            public string Name { get; }
            public string Type { get; }
            public bool IsRunning { get; }
            public IO IO { get; }
            public ErlangDetails Erlang { get; }
            public Mnesia Mnesia { get; }
            public MemoryDetails Memory { get; }
            public long NumberOfGarbageCollected { get; }
            public GCDetails GcDetails { get; }
            public long ReclaimedBytesFromGC { get; }
            public ReclaimedBytesFromGCDetails ReclaimedBytesFromGCDetails { get; }
            public ContextSwitchingDetails ContextSwitching { get; }
            public long ContextSwitches { get; }
            public ContextSwitchDetails ContextSwitchDetails { get; }
            public GarbageCollectionMetrics GarbageCollectionMetrics { get; }
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
            }

            public DiskDetails Disk { get; }
            public DiskUsageDetails Reads { get; }
            public DiskUsageDetails Writes { get; }
            public DiskUsageDetails Seeks { get; }
            public decimal AvgIOReadTime { get; }
            public AvgIOReadTimeDetails AvgIOReadTimeDetails { get; }
            public decimal AvgTimePerIOWrite { get; }
            public AvgTimePerIOWriteDetails AvgTimePerIOWriteDetails { get; }
            public long IOSyncCount { get; }
            public IOSyncCountDetails RateOfIOSyncs { get; }
            public decimal AverageIOSyncTime { get; }
            public AvgIOSyncTimeDetails AvgIOSyncTimeDetails { get; }
            public long IOSeekCount { get; }
            public IOSeekCountDetails RateOfIOSeeks { get; }
            public decimal AverageIOSeekTime { get; }
            public AvgIOSeekTimeDetails AvgIOSeekTimeDetails { get; }
            public long IOReopenCount { get; }
            public IOReopenCountDetails RateOfIOReopened { get; }

            
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
                public DiskUsageDetailsImpl(long total, decimal rate, long totalBytes, decimal bytesRate, long avgWallTime, decimal avgWallTimeRate)
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


        class QueueDetailsImpl :
            QueueDetails
        {
            public QueueDetailsImpl(MessageStats stats)
            {
                Published = new MessageDetailsImpl(stats.TotalMessagesPublished, stats.MessagesPublishedDetails.IsNull() ? 0 : stats.MessagesPublishedDetails.Rate);
                Confirmed = new MessageDetailsImpl(stats.TotalMessagesConfirmed, stats.MessagesConfirmedDetails.IsNull() ? 0 : stats.MessagesConfirmedDetails.Rate);
                UnroutableMessagesReturned = new MessageDetailsImpl(stats.TotalUnroutableMessagesReturned, stats.UnroutableMessagesReturnedDetails.IsNull() ? 0 : stats.UnroutableMessagesReturnedDetails.Rate);
                Gets = new MessageDetailsImpl(stats.TotalMessageGets, stats.MessageGetDetails.IsNull() ? 0 : stats.MessageGetDetails.Rate);
                GetsWithoutAcknowledgement = new MessageDetailsImpl(stats.TotalMessageGetsWithoutAck, stats.MessageGetsWithoutAckDetails.IsNull() ? 0 : stats.MessageGetsWithoutAckDetails.Rate);
                Delivered = new MessageDetailsImpl(stats.TotalMessagesDelivered, stats.MessageDeliveryDetails.IsNull() ? 0 : stats.MessageDeliveryDetails.Rate);
                DeliveredWithoutAcknowledgement = new MessageDetailsImpl(stats.TotalMessageDeliveredWithoutAck, stats.MessagesDeliveredWithoutAckDetails.IsNull() ? 0 : stats.MessagesDeliveredWithoutAckDetails.Rate);
                Redelivered = new MessageDetailsImpl(stats.TotalMessagesRedelivered, stats.MessagesRedeliveredDetails.IsNull() ? 0 : stats.MessagesRedeliveredDetails.Rate);
                Acknowledged = new MessageDetailsImpl(stats.TotalMessagesAcknowledged, stats.MessagesAcknowledgedDetails.IsNull() ? 0 : stats.MessagesAcknowledgedDetails.Rate);
                DeliveryGets = new MessageDetailsImpl(stats.TotalMessageDeliveryGets, stats.MessageDeliveryGetDetails.IsNull() ? 0 : stats.MessageDeliveryGetDetails.Rate);
            }

            public MessageDetails Published { get; }
            public MessageDetails Confirmed { get; }
            public MessageDetails UnroutableMessagesReturned { get; }
            public MessageDetails Gets { get; }
            public MessageDetails GetsWithoutAcknowledgement { get; }
            public MessageDetails Delivered { get; }
            public MessageDetails DeliveredWithoutAcknowledgement { get; }
            public MessageDetails Redelivered { get; }
            public MessageDetails Acknowledged { get; }
            public MessageDetails DeliveryGets { get; }

            
            class MessageDetailsImpl :
                MessageDetails
            {
                public MessageDetailsImpl(long total, decimal rate)
                {
                    Total = total;
                    Rate = rate;
                }

                public long Total { get; }
                public decimal Rate { get; }
            }
        }

        public string RabbitMqVersion { get; }
        public string ClusterName { get; }
        public IReadOnlyList<NodeStatus> Nodes { get; }
        public QueueDetails Queue { get; }
    }
}
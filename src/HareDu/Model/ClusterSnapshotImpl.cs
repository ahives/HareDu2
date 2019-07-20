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
    using System.Linq;
    using Core.Extensions;
    using Core.Model;

    class ClusterSnapshotImpl :
        ClusterSnapshot
    {
        public ClusterSnapshotImpl(ClusterInfo cluster, IReadOnlyList<NodeInfo> nodes,
            IReadOnlyList<ConnectionInfo> connections, IReadOnlyList<ChannelInfo> channels,
            IReadOnlyList<QueueInfo> queues)
        {
            ClusterName = cluster.ClusterName;
            RabbitMqVersion = cluster.RabbitMqVersion;
            Queue = new QueueMetricsImpl(cluster.MessageStats, queues);
            Nodes = GetNodes(cluster, nodes, connections, channels);
        }

        IReadOnlyList<NodeStatus> GetNodes(ClusterInfo cluster,
            IReadOnlyList<NodeInfo> nodes,
            IReadOnlyList<ConnectionInfo> connections,
            IReadOnlyList<ChannelInfo> channels)
        {
            return nodes
                .Select(node => new NodeStatusImpl(cluster, node, connections.FilterByNode(node.Name), channels))
                .Cast<NodeStatus>()
                .ToList();
        }

        public string RabbitMqVersion { get; }
        public string ClusterName { get; }
        public IReadOnlyList<NodeStatus> Nodes { get; }
        public QueueMetrics Queue { get; }
        public IReadOnlyList<QueueMetrics> Queues { get; }


        class NodeStatusImpl :
            NodeStatus
        {
            public NodeStatusImpl(ClusterInfo cluster, NodeInfo node, IEnumerable<ConnectionInfo> connections, IEnumerable<ChannelInfo> channels)
            {
                OS = new OperatingSystemMetricsImpl(node);
                Erlang = new ErlangMetricsImpl(cluster, node);
                IO = new IOImpl(cluster.MessageStats, node);
                ContextSwitching = new ContextSwitchDetailsImpl(node);
                Connections = connections
                    .Select(connection => new ConnectionMetricsImpl(connection, channels.FilterByConnection(connection.Name)))
                    .ToList();
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
            public MemoryDetails Memory { get; }
            public GarbageCollection GC { get; }
            public ContextSwitchingDetails ContextSwitching { get; }
            public IReadOnlyList<ConnectionMetrics> Connections { get; }

            
            class ConnectionMetricsImpl :
                ConnectionMetrics
            {
                public ConnectionMetricsImpl(ConnectionInfo connection, List<ChannelMetrics> channels)
                {
                    Traffic = new NetworkTrafficMetricsImpl(connection);
                    Channels = channels;
                }

                public NetworkTrafficMetrics Traffic { get; }
                public IReadOnlyList<ChannelMetrics> Channels { get; }

                
                class NetworkTrafficMetricsImpl :
                    NetworkTrafficMetrics
                {
                    public NetworkTrafficMetricsImpl(ConnectionInfo connection)
                    {
                        Sent = new PacketsImpl(connection.PacketsSent, connection.PacketsSentInOctets,
                            connection.RateOfPacketsSentInOctets?.Rate ?? 0);
                        Received = new PacketsImpl(connection.PacketsReceived, connection.PacketsReceivedInOctets,
                            connection.RateOfPacketsReceivedInOctets?.Rate ?? 0);
                    }

                    
                    class PacketsImpl :
                        Packets
                    {
                        public PacketsImpl(long total, long octets, decimal rate)
                        {
                            Total = total;
                            Octets = octets;
                            Rate = rate;
                        }

                        public long Total { get; }
                        public long Octets { get; }
                        public decimal Rate { get; }
                    }

                    public Packets Sent { get; }
                    public Packets Received { get; }
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

            
            class ErlangMetricsImpl :
                ErlangMetrics
            {
                public ErlangMetricsImpl(ClusterInfo cluster, NodeInfo node)
                {
                    Version = cluster.ErlangVerion;
                    MemoryUsed = node.MemoryUsed;
                    AvailableCores = node.Processors;
                    Processes = new ErlangProcessMetricsImpl(node.TotalProcesses, node.ProcessesUsed,
                        node.ProcessUsageDetails?.Rate ?? 0);
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
                        UsageRate = node.SocketsUsedDetails.Rate;
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
                        UsageRate = node.FileDescriptorUsedDetails.Rate;
                        OpenAttempts = node.TotalOpenFileHandleAttempts;
                        OpenAttemptRate = node.FileHandleOpenAttemptCountDetails.Rate;
                        OpenAttemptAvgTime = node.FileHandleOpenAttemptAvgTimeDetails.Rate;
                        OpenAttemptAvgTimeRate = node.FileHandleOpenAttemptAvgTimeDetails.Rate;
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


        class QueueMetricsImpl :
            QueueMetrics
        {
            public QueueMetricsImpl(MessageStats stats, IReadOnlyList<QueueInfo> queues)
            {
                Published = new MessageDetailsImpl(stats.TotalMessagesPublished, stats.MessagesPublishedDetails.IsNull() ? 0 : stats.MessagesPublishedDetails.Rate);
                Confirmed = new MessageDetailsImpl(stats.TotalMessagesConfirmed, stats.MessagesConfirmedDetails.IsNull() ? 0 : stats.MessagesConfirmedDetails.Rate);
                Unroutable = new MessageDetailsImpl(stats.TotalUnroutableMessages, stats.UnroutableMessagesDetails.IsNull() ? 0 : stats.UnroutableMessagesDetails.Rate);
                Gets = new MessageDetailsImpl(stats.TotalMessageGets, stats.MessageGetDetails.IsNull() ? 0 : stats.MessageGetDetails.Rate);
                GetsWithoutAck = new MessageDetailsImpl(stats.TotalMessageGetsWithoutAck, stats.MessageGetsWithoutAckDetails.IsNull() ? 0 : stats.MessageGetsWithoutAckDetails.Rate);
                Delivered = new MessageDetailsImpl(stats.TotalMessagesDelivered, stats.MessageDeliveryDetails.IsNull() ? 0 : stats.MessageDeliveryDetails.Rate);
                DeliveredWithoutAck = new MessageDetailsImpl(stats.TotalMessageDeliveredWithoutAck, stats.MessagesDeliveredWithoutAckDetails.IsNull() ? 0 : stats.MessagesDeliveredWithoutAckDetails.Rate);
                Redelivered = new MessageDetailsImpl(stats.TotalMessagesRedelivered, stats.MessagesRedeliveredDetails.IsNull() ? 0 : stats.MessagesRedeliveredDetails.Rate);
                Acknowledged = new MessageDetailsImpl(stats.TotalMessagesAcknowledged, stats.MessagesAcknowledgedDetails.IsNull() ? 0 : stats.MessagesAcknowledgedDetails.Rate);
                DeliveryGets = new MessageDetailsImpl(stats.TotalMessageDeliveryGets, stats.MessageDeliveryGetDetails.IsNull() ? 0 : stats.MessageDeliveryGetDetails.Rate);
            }

            public MessageDetails Published { get; }
            public MessageDetails Confirmed { get; }
            public MessageDetails Unroutable { get; }
            public MessageDetails Gets { get; }
            public MessageDetails GetsWithoutAck { get; }
            public MessageDetails Delivered { get; }
            public MessageDetails DeliveredWithoutAck { get; }
            public MessageDetails Redelivered { get; }
            public MessageDetails Acknowledged { get; }
            public MessageDetails Unacknowledged { get; }
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
    }
}
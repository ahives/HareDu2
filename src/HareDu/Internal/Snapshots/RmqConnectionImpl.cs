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
namespace HareDu.Internal.Snapshots
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Core.Model;
    using Model;

    class RmqConnectionImpl :
        BaseSnapshot,
        RmqConnection
    {
        public RmqConnectionImpl(IResourceFactory factory)
            : base(factory)
        {
        }

        public async Task<ConnectionSnapshot> Get(CancellationToken cancellationToken = default)
        {
            Result<ClusterInfo> clusterResource = await _factory
                .Resource<Cluster>()
                .GetDetails(cancellationToken);

            if (clusterResource.HasFaulted)
            {
                // TODO: Handle error scenario
                return default;
            }

            var cluster = clusterResource.Select(x => x.Data);
            
            ResultList<ConnectionInfo> connectionResource = await _factory
                .Resource<Connection>()
                .GetAll(cancellationToken);

            if (connectionResource.HasFaulted)
            {
                // TODO: Handle error scenario
                return default;
            }
            
            var connections = connectionResource.Select(x => x.Data);

            var channelResource = await _factory
                .Resource<Channel>()
                .GetAll(cancellationToken);

            if (channelResource.HasFaulted)
            {
                // TODO: Handle error scenario
                return default;
            }

            var channels = channelResource.Select(x => x.Data);

            return new ConnectionSnapshotImpl(cluster, connections, channels);
        }

        
        class ConnectionSnapshotImpl :
            ConnectionSnapshot
        {
            public ConnectionSnapshotImpl(ClusterInfo cluster, IReadOnlyList<ConnectionInfo> connections,
                IReadOnlyList<ChannelInfo> channels)
            {
                ChannelsClosed = new ChurnMetricsImpl(cluster.ChurnRates?.TotalChannelsClosed ?? 0, cluster.ChurnRates?.ClosedChannelDetails?.Rate ?? 0);
                ChannelsCreated = new ChurnMetricsImpl(cluster.ChurnRates?.TotalChannelsCreated ?? 0, cluster.ChurnRates?.CreatedChannelDetails?.Rate ?? 0);
                ConnectionsCreated = new ChurnMetricsImpl(cluster.ChurnRates?.TotalConnectionsCreated ?? 0, cluster.ChurnRates?.CreatedConnectionDetails?.Rate ?? 0);
                ConnectionsClosed = new ChurnMetricsImpl(cluster.ChurnRates?.TotalConnectionsClosed ?? 0, cluster.ChurnRates?.ClosedConnectionDetails?.Rate ?? 0);
                Connections = connections
                    .Select(x => new ConnectionMetricsImpl(x, channels.FilterByConnection(x.Name)))
                    .Cast<ConnectionMetrics>()
                    .ToList();
            }

            public ChurnMetrics ChannelsClosed { get; }
            public ChurnMetrics ChannelsCreated { get; }
            public ChurnMetrics ConnectionsClosed { get; }
            public ChurnMetrics ConnectionsCreated { get; }
            public IReadOnlyList<ConnectionMetrics> Connections { get; }

            
            class ChurnMetricsImpl :
                ChurnMetrics
            {
                public ChurnMetricsImpl(int total, decimal rate)
                {
                    Total = total;
                    Rate = rate;
                }

                public long Total { get; }
                public decimal Rate { get; }
            }

            
            class ConnectionMetricsImpl :
                ConnectionMetrics
            {
                public ConnectionMetricsImpl(ConnectionInfo connection, IReadOnlyList<ChannelMetrics> channels)
                {
                    Identifier = connection.Name;
                    NetworkTraffic = new NetworkTrafficMetricsImpl(connection);
                    Channels = channels;
                    TotalChannels = connection.Channels;
                    ChannelLimit = connection.MaxChannels;
                    Node = connection.Node;
                }

                public string Identifier { get; }
                public NetworkTrafficMetrics NetworkTraffic { get; }
                public long ChannelLimit { get; }
                public long TotalChannels { get; }
                public string Node { get; }
                public IReadOnlyList<ChannelMetrics> Channels { get; }

                
                class NetworkTrafficMetricsImpl :
                    NetworkTrafficMetrics
                {
                    public NetworkTrafficMetricsImpl(ConnectionInfo connection)
                    {
                        Sent = new PacketsImpl(connection.PacketsSent, connection.PacketBytesSent,
                            connection.RateOfPacketsSentInOctets?.Rate ?? 0);
                        Received = new PacketsImpl(connection.PacketsReceived, connection.PacketBytesReceived,
                            connection.RateOfPacketsReceivedInOctets?.Rate ?? 0);
                    }

                    
                    class PacketsImpl :
                        Packets
                    {
                        public PacketsImpl(long total, long octets, decimal rate)
                        {
                            Total = total;
                            Bytes = octets;
                            Rate = rate;
                        }

                        public long Total { get; }
                        public long Bytes { get; }
                        public decimal Rate { get; }
                    }

                    public Packets Sent { get; }
                    public Packets Received { get; }
                }
            }
        }
    }
}
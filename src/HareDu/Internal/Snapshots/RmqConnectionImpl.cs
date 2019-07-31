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
            Result<ConnectionInfo> connectionResource = await _factory
                .Resource<Connection>()
                .GetAll(cancellationToken);

            var connections = connectionResource.Select(x => x.Data);

            var channelResource = await _factory
                .Resource<Channel>()
                .GetAll(cancellationToken);

            var channels = channelResource.Select(x => x.Data);

            return new ConnectionSnapshotImpl(connections, channels);
        }

        
        class ConnectionSnapshotImpl :
            ConnectionSnapshot
        {
            public ConnectionSnapshotImpl(IReadOnlyList<ConnectionInfo> connections, IReadOnlyList<ChannelInfo> channels)
            {
                Connections = connections
                    .Select(x => new ConnectionMetricsImpl(x, channels.FilterByConnection(x.Name)))
                    .Cast<ConnectionMetrics>()
                    .ToList();
            }

            public IReadOnlyList<ConnectionMetrics> Connections { get; }

            
            class ConnectionMetricsImpl :
                ConnectionMetrics
            {
                public ConnectionMetricsImpl(ConnectionInfo connection, IReadOnlyList<ChannelMetrics> channels)
                {
                    Identifier = connection.Name;
                    NetworkTraffic = new NetworkTrafficMetricsImpl(connection);
                    Channels = channels;
                    ChannelLimit = connection.MaxChannels;
                }

                public string Identifier { get; }
                public NetworkTrafficMetrics NetworkTraffic { get; }
                public long ChannelLimit { get; }
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
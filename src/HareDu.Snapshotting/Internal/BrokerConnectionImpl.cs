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
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Core.Model;
    using Extensions;
    using Model;

    class BrokerConnectionImpl :
        BaseSnapshot<BrokerConnectivitySnapshot>,
        BrokerConnection
    {
        readonly List<IDisposable> _observers;
        readonly List<Result<BrokerConnectivitySnapshot>> _snapshots;

        public IReadOnlyList<Result<BrokerConnectivitySnapshot>> Snapshots => _snapshots;

        public BrokerConnectionImpl(IResourceFactory factory)
            : base(factory)
        {
            _observers = new List<IDisposable>();
            _snapshots = new List<Result<BrokerConnectivitySnapshot>>();
        }

        public ResourceSnapshot<BrokerConnectivitySnapshot> TakeSnapshot(CancellationToken cancellationToken = default)
        {
            var cluster = _factory
                .Object<Cluster>()
                .GetDetails(cancellationToken)
                .Select(x => x.Data);
            
            var connections = _factory
                .Object<Connection>()
                .GetAll(cancellationToken)
                .Select(x => x.Data);

            var channels = _factory
                .Object<Channel>()
                .GetAll(cancellationToken)
                .Select(x => x.Data);
            
            BrokerConnectivitySnapshot data = new BrokerConnectivitySnapshotImpl(cluster, connections, channels);

            NotifyObservers(data);
            
            var snapshot = new SuccessfulResult<BrokerConnectivitySnapshot>(data, null);

            _snapshots.Add(snapshot);

            return this;
        }

        public ResourceSnapshot<BrokerConnectivitySnapshot> RegisterObserver(IObserver<SnapshotContext<BrokerConnectivitySnapshot>> observer)
        {
            if (observer != null)
            {
                _observers.Add(Subscribe(observer));
            }

            return this;
        }

        public ResourceSnapshot<BrokerConnectivitySnapshot> RegisterObservers(
            IReadOnlyList<IObserver<SnapshotContext<BrokerConnectivitySnapshot>>> observers)
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


        class BrokerConnectivitySnapshotImpl :
            BrokerConnectivitySnapshot
        {
            public BrokerConnectivitySnapshotImpl(ClusterInfo cluster, IReadOnlyList<ConnectionInfo> connections,
                IReadOnlyList<ChannelInfo> channels)
            {
                ChannelsClosed = new ChurnMetricsImpl(cluster.ChurnRates?.TotalChannelsClosed ?? 0, cluster.ChurnRates?.ClosedChannelDetails?.Rate ?? 0);
                ChannelsCreated = new ChurnMetricsImpl(cluster.ChurnRates?.TotalChannelsCreated ?? 0, cluster.ChurnRates?.CreatedChannelDetails?.Rate ?? 0);
                ConnectionsCreated = new ChurnMetricsImpl(cluster.ChurnRates?.TotalConnectionsCreated ?? 0, cluster.ChurnRates?.CreatedConnectionDetails?.Rate ?? 0);
                ConnectionsClosed = new ChurnMetricsImpl(cluster.ChurnRates?.TotalConnectionsClosed ?? 0, cluster.ChurnRates?.ClosedConnectionDetails?.Rate ?? 0);
                Connections = connections
                    .Select(x => new ConnectionSnapshotImpl(x, channels.FilterByConnection(x.Name)))
                    .Cast<ConnectionSnapshot>()
                    .ToList();
            }

            public ChurnMetrics ChannelsClosed { get; }
            public ChurnMetrics ChannelsCreated { get; }
            public ChurnMetrics ConnectionsClosed { get; }
            public ChurnMetrics ConnectionsCreated { get; }
            public IReadOnlyList<ConnectionSnapshot> Connections { get; }

            
            class ChurnMetricsImpl :
                ChurnMetrics
            {
                public ChurnMetricsImpl(ulong total, decimal rate)
                {
                    Total = total;
                    Rate = rate;
                }

                public ulong Total { get; }
                public decimal Rate { get; }
            }

            
            class ConnectionSnapshotImpl :
                ConnectionSnapshot
            {
                public ConnectionSnapshotImpl(ConnectionInfo connection, IReadOnlyList<ChannelSnapshot> channels)
                {
                    Identifier = connection.Name;
                    NetworkTraffic = new NetworkTrafficSnapshotImpl(connection);
                    Channels = channels;
                    ChannelLimit = connection.MaxChannels;
                    Node = connection.Node;
                    VirtualHost = connection.VirtualHost;
                    State = connection.State.Convert();
                }

                public string Identifier { get; }
                public NetworkTrafficSnapshot NetworkTraffic { get; }
                public ulong ChannelLimit { get; }
                public string Node { get; }
                public string VirtualHost { get; }
                public ConnectionState State { get; }
                public IReadOnlyList<ChannelSnapshot> Channels { get; }

                
                class NetworkTrafficSnapshotImpl :
                    NetworkTrafficSnapshot
                {
                    public NetworkTrafficSnapshotImpl(ConnectionInfo connection)
                    {
                        MaxFrameSize = connection.MaxFrameSizeInBytes;
                        Sent = new PacketsImpl(connection.PacketsSent, connection.PacketBytesSent,
                            connection.RateOfPacketBytesSent?.Rate ?? 0);
                        Received = new PacketsImpl(connection.PacketsReceived, connection.PacketBytesReceived,
                            connection.RateOfPacketBytesReceived?.Rate ?? 0);
                    }

                    
                    class PacketsImpl :
                        Packets
                    {
                        public PacketsImpl(ulong total, ulong bytes, decimal rate)
                        {
                            Total = total;
                            Bytes = bytes;
                            Rate = rate;
                        }

                        public ulong Total { get; }
                        public ulong Bytes { get; }
                        public decimal Rate { get; }
                    }

                    public ulong MaxFrameSize { get; }
                    public Packets Sent { get; }
                    public Packets Received { get; }
                }
            }
        }
    }
}
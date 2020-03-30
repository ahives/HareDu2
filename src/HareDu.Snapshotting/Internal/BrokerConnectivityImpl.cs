// Copyright 2013-2020 Albert L. Hives
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
    using Extensions;
    using HareDu.Model;
    using HareDu.Registration;
    using MassTransit;
    using Model;

    class BrokerConnectivityImpl :
        BaseSnapshotLens<BrokerConnectivitySnapshot>,
        SnapshotLens<BrokerConnectivitySnapshot>
    {
        readonly List<IDisposable> _observers;

        public SnapshotHistory<BrokerConnectivitySnapshot> History => _timeline.Value;

        public BrokerConnectivityImpl(IBrokerObjectFactory factory)
            : base(factory)
        {
            _observers = new List<IDisposable>();
        }

        public SnapshotResult<BrokerConnectivitySnapshot> TakeSnapshot(CancellationToken cancellationToken = default)
        {
            var cluster = _factory
                .Object<SystemOverview>()
                .Get(cancellationToken)
                .GetResult();

            if (cluster.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve cluster information."));
                
                return new EmptySnapshotResult<BrokerConnectivitySnapshot>();
            }

            var connections = _factory
                .Object<Connection>()
                .GetAll(cancellationToken)
                .GetResult();

            if (connections.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve connection information."));
                
                return new EmptySnapshotResult<BrokerConnectivitySnapshot>();
            }

            var channels = _factory
                .Object<Channel>()
                .GetAll(cancellationToken)
                .GetResult();

            if (channels.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve channel information."));
                
                return new EmptySnapshotResult<BrokerConnectivitySnapshot>();
            }
            
            BrokerConnectivitySnapshot snapshot = new BrokerConnectivitySnapshotImpl(
                cluster.Select(x => x.Data),
                connections.Select(x => x.Data),
                channels.Select(x => x.Data));

            string identifier = NewId.Next().ToString();
            DateTimeOffset timestamp = DateTimeOffset.UtcNow;

            SaveSnapshot(identifier, snapshot, timestamp);
            NotifyObservers(identifier, snapshot, timestamp);

            return new SnapshotResultImpl(identifier, snapshot, timestamp);
        }

        public SnapshotLens<BrokerConnectivitySnapshot> RegisterObserver(IObserver<SnapshotContext<BrokerConnectivitySnapshot>> observer)
        {
            if (observer != null)
            {
                _observers.Add(Subscribe(observer));
            }

            return this;
        }

        public SnapshotLens<BrokerConnectivitySnapshot> RegisterObservers(
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
            public BrokerConnectivitySnapshotImpl(SystemOverviewInfo systemOverview, IReadOnlyList<ConnectionInfo> connections,
                IReadOnlyList<ChannelInfo> channels)
            {
                ClusterName = systemOverview.ClusterName;
                BrokerVersion = systemOverview.RabbitMqVersion;
                ChannelsClosed = new ChurnMetricsImpl(systemOverview.ChurnRates?.TotalChannelsClosed ?? 0, systemOverview.ChurnRates?.ClosedChannelDetails?.Rate ?? 0);
                ChannelsCreated = new ChurnMetricsImpl(systemOverview.ChurnRates?.TotalChannelsCreated ?? 0, systemOverview.ChurnRates?.CreatedChannelDetails?.Rate ?? 0);
                ConnectionsCreated = new ChurnMetricsImpl(systemOverview.ChurnRates?.TotalConnectionsCreated ?? 0, systemOverview.ChurnRates?.CreatedConnectionDetails?.Rate ?? 0);
                ConnectionsClosed = new ChurnMetricsImpl(systemOverview.ChurnRates?.TotalConnectionsClosed ?? 0, systemOverview.ChurnRates?.ClosedConnectionDetails?.Rate ?? 0);
                Connections = connections
                    .Select(x => new ConnectionSnapshotImpl(x, channels.FilterByConnection(x.Name)))
                    .Cast<ConnectionSnapshot>()
                    .ToList();
            }

            public string BrokerVersion { get; }
            public string ClusterName { get; }
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
                    OpenChannelsLimit = connection.OpenChannelsLimit;
                    NodeIdentifier = connection.Node;
                    VirtualHost = connection.VirtualHost;
                    State = connection.State.Convert();
                }

                public string Identifier { get; }
                public NetworkTrafficSnapshot NetworkTraffic { get; }
                public ulong OpenChannelsLimit { get; }
                public string NodeIdentifier { get; }
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
                            connection.PacketBytesSentDetails?.Rate ?? 0);
                        Received = new PacketsImpl(connection.PacketsReceived, connection.PacketBytesReceived,
                            connection.PacketBytesReceivedDetails?.Rate ?? 0);
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
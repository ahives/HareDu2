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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Core.Model;
    using HareDu.Diagnostics;
    using Model;

    class RmqBrokerConnectionImpl :
        BaseSnapshot,
        RmqBrokerConnection
    {
        readonly List<IDiagnosticCheck<ChannelSnapshot>> _channelDiagnosticChecks;
        Result<ConnectivitySnapshot> _result;

        public Result<ConnectivitySnapshot> Snapshot => _result;
        public IReadOnlyList<DiagnosticResult> DiagnosticResults { get; private set; }
        
        public RmqBrokerConnectionImpl(IResourceFactory factory)
            : base(factory)
        {
            _channelDiagnosticChecks = GetDiagnosticChecks<ChannelSnapshot>().ToList();

            DiagnosticResults = new List<DiagnosticResult>();
        }

        public RmqBrokerConnectionImpl(IResourceFactory factory, IList<IObserver<DiagnosticContext>> observers)
            : base(factory)
        {
            _channelDiagnosticChecks = GetDiagnosticChecks<ChannelSnapshot>().ToList();
            
            DiagnosticResults = new List<DiagnosticResult>();

            ConnectObservers(observers, _channelDiagnosticChecks);
        }

        public async Task<Result<ConnectivitySnapshot>> Get(CancellationToken cancellationToken = default)
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
            
            return new SuccessfulResult<ConnectivitySnapshot>(new ConnectivitySnapshotImpl(cluster, connections, channels), null);
        }

        public RmqBrokerConnection Execute(CancellationToken cancellationToken = default)
        {
            var cluster = _factory
                .Resource<Cluster>()
                .GetDetails(cancellationToken)
                .Select(x => x.Data);
            
            var connections = _factory
                .Resource<Connection>()
                .GetAll(cancellationToken)
                .Select(x => x.Data);

            var channels = _factory
                .Resource<Channel>()
                .GetAll(cancellationToken)
                .Select(x => x.Data);
            
            _result = new SuccessfulResult<ConnectivitySnapshot>(new ConnectivitySnapshotImpl(cluster, connections, channels), null);

            return this;
        }

        public RmqBrokerConnection RunDiagnostics()
        {
            var snapshot = _result.Select(x => x.Data);
            var diagnosticResults = new List<DiagnosticResult>();
            
            for (int i = 0; i < snapshot.Connections.Count; i++)
            {
                for (int j = 0; j < snapshot.Connections[i].Channels.Count; j++)
                {
                    for (int k = 0; k < _channelDiagnosticChecks.Count; k++)
                    {
                        diagnosticResults.Add(_channelDiagnosticChecks[k].Execute(snapshot.Connections[i].Channels[j]));
                    }
                }
            }

            DiagnosticResults = diagnosticResults;

            return this;
        }


        public IEnumerable<DiagnosticResult> RunDiagnostics(ConnectivitySnapshot snapshot)
        {
            for (int i = 0; i < snapshot.Connections.Count; i++)
            {
                for (int j = 0; j < snapshot.Connections[i].Channels.Count; j++)
                {
                    for (int k = 0; k < _channelDiagnosticChecks.Count; k++)
                    {
                        DiagnosticResult result = _channelDiagnosticChecks[k].Execute(snapshot.Connections[i].Channels[j]);

                        yield return result;
                    }
                }
            }
        }


        class ConnectivitySnapshotImpl :
            ConnectivitySnapshot
        {
            public ConnectivitySnapshotImpl(ClusterInfo cluster, IReadOnlyList<ConnectionInfo> connections,
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
                public ChurnMetricsImpl(int total, decimal rate)
                {
                    Total = total;
                    Rate = rate;
                }

                public long Total { get; }
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
                }

                public string Identifier { get; }
                public NetworkTrafficSnapshot NetworkTraffic { get; }
                public long ChannelLimit { get; }
                public string Node { get; }
                public string VirtualHost { get; }
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
                        public PacketsImpl(long total, long bytes, decimal rate)
                        {
                            Total = total;
                            Bytes = bytes;
                            Rate = rate;
                        }

                        public long Total { get; }
                        public long Bytes { get; }
                        public decimal Rate { get; }
                    }

                    public long MaxFrameSize { get; }
                    public Packets Sent { get; }
                    public Packets Received { get; }
                }
            }
        }
    }
}
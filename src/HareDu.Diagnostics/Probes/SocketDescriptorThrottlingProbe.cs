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
namespace HareDu.Diagnostics.Probes
{
    using System;
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class SocketDescriptorThrottlingProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Socket Descriptor Throttling Probe";
        public string Description =>
            "Checks network to see if the number of sockets currently in use is less than or equal to the number available.";
        public ComponentType ComponentType => ComponentType.Node;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.Throughput;
        public ProbeStatus Status => _status;

        public SocketDescriptorThrottlingProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
            _status = !_config.IsNull() ? ProbeStatus.Online : ProbeStatus.Offline;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            NodeSnapshot data = snapshot as NodeSnapshot;

            ulong warningThreshold = ComputeWarningThreshold(data.OS.SocketDescriptors.Available);
            
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("OS.Sockets.Available", data.OS.SocketDescriptors.Available.ToString()),
                new ProbeDataImpl("OS.Sockets.Used", data.OS.SocketDescriptors.Used.ToString()),
                new ProbeDataImpl("CalculatedThreshold", warningThreshold.ToString())
            };

            if (data.OS.SocketDescriptors.Used < warningThreshold && warningThreshold < data.OS.SocketDescriptors.Available)
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.ClusterIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }
            else if (data.OS.SocketDescriptors.Used == data.OS.SocketDescriptors.Available)
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.ClusterIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Warning, out var article);
                result = new WarningProbeResult(data.ClusterIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }

            NotifyObservers(result);
                
            return result;
        }

        ulong ComputeWarningThreshold(ulong socketsAvailable)
            => _config.Probes.SocketUsageThresholdCoefficient >= 1
                ? socketsAvailable
                : Convert.ToUInt64(Math.Ceiling(socketsAvailable * _config.Probes.SocketUsageThresholdCoefficient));
    }
}
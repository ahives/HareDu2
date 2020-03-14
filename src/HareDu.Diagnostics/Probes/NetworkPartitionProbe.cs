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
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class NetworkPartitionProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Network Partition Probe";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Node;
        public ProbeCategory Category => ProbeCategory.Connectivity;

        public NetworkPartitionProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            NodeSnapshot data = snapshot as NodeSnapshot;
            ProbeResult result;

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("NetworkPartitions", data.NetworkPartitions.ToString())
            };
            
            if (data.NetworkPartitions.Any())
            {
                _kb.TryGet(Identifier, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.ClusterIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Identifier, ProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.ClusterIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }

            NotifyObservers(result);
                
            return result;
        }
    }
}
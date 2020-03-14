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
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class BlockedConnectionProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public string Id => GetType().GetIdentifier();
        public string Name => "Blocked Connection Probe";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Connection;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public BlockedConnectionProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            ConnectionSnapshot data = snapshot as ConnectionSnapshot;

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("State", data.State.ToString())
            };
            
            if (data.State == ConnectionState.Blocked)
            {
                _kb.TryGet(Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.NodeIdentifier,
                    data.Identifier,
                    Id,
                    Name,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Id, ProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.NodeIdentifier,
                    data.Identifier,
                    Id,
                    Name,
                    ComponentType,
                    probeData,
                    article);
            }

            NotifyObservers(result);

            return result;
        }
    }
}
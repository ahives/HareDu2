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
    using Core.Configuration;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class RuntimeProcessLimitProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Runtime Process Limit Probe";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Runtime;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.Throughput;

        public RuntimeProcessLimitProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            BrokerRuntimeSnapshot data = snapshot as BrokerRuntimeSnapshot;

            if (_config.IsNull())
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.NA, out var article);
                result = new NotApplicableProbeResult(null,
                    null,
                    Identifier,
                    ComponentType,
                    article);

                NotifyObservers(result);

                return result;
            }

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Processes.Limit", data.Processes.Limit.ToString()),
                new ProbeDataImpl("Processes.Used", data.Processes.Used.ToString())
            };

            if (data.Processes.Used < data.Processes.Limit)
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.ClusterIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.ClusterIdentifier,
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
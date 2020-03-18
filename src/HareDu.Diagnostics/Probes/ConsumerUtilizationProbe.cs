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
    using Registration;
    using Snapshotting.Model;

    public class ConsumerUtilizationProbe :
        BaseDiagnosticProbe,
        IRefreshConfiguration,
        DiagnosticProbe
    {
        DiagnosticsConfig _config;
        
        public string Id => GetType().GetIdentifier();
        public string Name => "Consumer Utilization Probe";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Queue;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public ConsumerUtilizationProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            QueueSnapshot data = snapshot as QueueSnapshot;

            if (_config.IsNull() || _config.Probes.IsNull())
            {
                _kb.TryGet(Id, ProbeResultStatus.NA, out var article);
                result = new NotApplicableProbeResult(!data.IsNull() ? data.Node : null,
                    !data.IsNull() ? data.Identifier : null,
                    Id,
                    Name,
                    ComponentType,
                    article);

                NotifyObservers(result);

                return result;
            }

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("ConsumerUtilization", data.ConsumerUtilization.ToString()),
                new ProbeDataImpl("ConsumerUtilizationThreshold", _config.Probes.ConsumerUtilizationThreshold.ToString())
            };
            
            if (data.ConsumerUtilization >= _config.Probes.ConsumerUtilizationThreshold
                && data.ConsumerUtilization < 1.0M
                && _config.Probes.ConsumerUtilizationThreshold <= 1.0M)
            {
                _kb.TryGet(Id, ProbeResultStatus.Warning, out var article);
                result = new WarningProbeResult(data.Node,
                    data.Identifier,
                    Id,
                    Name,
                    ComponentType,
                    probeData,
                    article);
            }
            else if (data.ConsumerUtilization < _config.Probes.ConsumerUtilizationThreshold
                     && _config.Probes.ConsumerUtilizationThreshold <= 1.0M)
            {
                _kb.TryGet(Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.Node,
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
                result = new HealthyProbeResult(data.Node,
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

        public void RefreshConfig(DiagnosticsConfig config) => _config = config;
    }
}
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

    public class UnroutableMessageProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Unroutable Message Probe";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Exchange;
        public ProbeCategory Category => ProbeCategory.Efficiency;

        public UnroutableMessageProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            BrokerQueuesSnapshot data = snapshot as BrokerQueuesSnapshot;
            
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Churn.NotRouted.Total", data.Churn.NotRouted.Total.ToString())
            };

            if (data.Churn.NotRouted.Total > 0)
            {
                _kb.TryGet(Identifier, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.ClusterName,
                    null,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Identifier, ProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.ClusterName,
                    null,
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
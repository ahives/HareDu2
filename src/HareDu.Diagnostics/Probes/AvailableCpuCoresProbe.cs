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
    using Internal;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class AvailableCpuCoresProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Available CPU Cores Probe";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Node;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.Throughput;
        public DiagnosticProbeStatus Status => _status;

        public AvailableCpuCoresProbe(IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
            _status = DiagnosticProbeStatus.Online;
        }

        public DiagnosticProbeResult Execute<T>(T snapshot)
        {
            DiagnosticProbeResult result;
            NodeSnapshot data = snapshot as NodeSnapshot;

            var probeData = new List<DiagnosticProbeData>
            {
                new DiagnosticProbeDataImpl("AvailableCoresDetected", data.AvailableCoresDetected.ToString())
            };

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            if (data.AvailableCoresDetected <= 0)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Unhealthy, out knowledgeBaseArticle);
                result = new NegativeDiagnosticProbeResult(data.ClusterIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Healthy, out knowledgeBaseArticle);
                result = new PositiveDiagnosticProbeResult(data.ClusterIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    knowledgeBaseArticle);
            }

            NotifyObservers(result);
                
            return result;
        }
    }
}
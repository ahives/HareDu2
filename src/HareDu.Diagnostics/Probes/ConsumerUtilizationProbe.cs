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

    public class ConsumerUtilizationProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Consumer Utilization Analyzer";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Queue;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.Throughput;
        public DiagnosticProbeStatus Status => _status;

        public ConsumerUtilizationProbe(DiagnosticsConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
            _config = config;
            _status = !_config.IsNull() ? DiagnosticProbeStatus.Online : DiagnosticProbeStatus.Offline;
        }

        public DiagnosticProbeResult Execute<T>(T snapshot)
        {
            DiagnosticProbeResult result;
            QueueSnapshot data = snapshot as QueueSnapshot;

            var probeData = new List<DiagnosticProbeData>
            {
                new DiagnosticProbeDataImpl("ConsumerUtilization", data.ConsumerUtilization.ToString()),
                new DiagnosticProbeDataImpl("Analyzer.ConsumerUtilizationWarningCoefficient", _config.ConsumerUtilizationWarningCoefficient.ToString())
            };

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            if (data.ConsumerUtilization >= _config.ConsumerUtilizationWarningCoefficient && data.ConsumerUtilization < 1.0M)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticProbeResult(data.Node,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    knowledgeBaseArticle);
            }
            else if (data.ConsumerUtilization < _config.ConsumerUtilizationWarningCoefficient)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticProbeResult(data.Node,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticProbeResult(data.Node,
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
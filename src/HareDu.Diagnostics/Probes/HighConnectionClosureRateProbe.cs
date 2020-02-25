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

    public class HighConnectionClosureRateProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        public string Identifier => GetType().GetIdentifier();
        public string Name => "High Connection Closure Rate Probe";
        public string Description => "";
        public ComponentType ComponentType => ComponentType.Connection;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.Connectivity;
        public DiagnosticProbeStatus Status => _status;

        public HighConnectionClosureRateProbe(DiagnosticsConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
            _config = config;
            _status = !_config.IsNull() ? DiagnosticProbeStatus.Online : DiagnosticProbeStatus.Offline;
        }

        public DiagnosticProbeResult Execute<T>(T snapshot)
        {
            DiagnosticProbeResult result;
            BrokerConnectivitySnapshot data = snapshot as BrokerConnectivitySnapshot;

            var probeData = new List<DiagnosticProbeData>
            {
                new DiagnosticProbeDataImpl("ConnectionsClosed.Rate", data.ConnectionsClosed.Rate.ToString()),
                new DiagnosticProbeDataImpl("HighClosureRateThreshold",
                    _config.HighClosureRateWarningThreshold.ToString())
            };

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            if (data.ConnectionsClosed.Rate >= _config.HighClosureRateWarningThreshold)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Warning, out knowledgeBaseArticle);
                result = new WarningDiagnosticProbeResult(null, null, Identifier, ComponentType, probeData, knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Healthy, out knowledgeBaseArticle);
                result = new PositiveDiagnosticProbeResult(null, null, Identifier, ComponentType, probeData, knowledgeBaseArticle);
            }

            NotifyObservers(result);

            return result;
        }
    }
}
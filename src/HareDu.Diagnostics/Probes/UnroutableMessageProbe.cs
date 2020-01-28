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

    public class UnroutableMessageProbe :
        BaseDiagnosticProbe,
        IDiagnosticProbe
    {
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Unroutable Message Analyzer";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Exchange;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.Efficiency;
        public DiagnosticProbeStatus Status => _status;

        public UnroutableMessageProbe(DiagnosticsConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
            _status = DiagnosticProbeStatus.Online;
        }

        public DiagnosticProbeResult Execute<T>(T snapshot)
        {
            DiagnosticProbeResult result;
            BrokerQueuesSnapshot data = snapshot as BrokerQueuesSnapshot;

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            var analyzerData = new List<DiagnosticProbeData>
            {
                new DiagnosticProbeDataImpl("Churn.NotRouted.Total", data.Churn.NotRouted.Total.ToString())
            };

            if (data.Churn.NotRouted.Total > 0)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticProbeResult(data.ClusterName,
                    null,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticProbeResult(data.ClusterName,
                    null,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }

            NotifyObservers(result);
                
            return result;
        }
    }
}
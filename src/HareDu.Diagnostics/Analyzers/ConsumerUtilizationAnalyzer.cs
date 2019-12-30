// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Diagnostics.Analyzers
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Extensions;
    using Internal;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class ConsumerUtilizationAnalyzer :
        BaseDiagnosticAnalyzer,
        IDiagnosticAnalyzer
    {
        readonly DiagnosticAnalyzerConfig _config;
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Consumer Utilization Analyzer";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Queue;
        public DiagnosticAnalyzerCategory Category => DiagnosticAnalyzerCategory.Throughput;
        public DiagnosticAnalyzerStatus Status => _status;

        public ConsumerUtilizationAnalyzer(DiagnosticAnalyzerConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
            _config = config;
            _status = !_config.IsNull() ? DiagnosticAnalyzerStatus.Online : DiagnosticAnalyzerStatus.Offline;
        }

        public DiagnosticAnalyzerResult Execute<T>(T snapshot)
        {
            DiagnosticAnalyzerResult result;
            QueueSnapshot data = snapshot as QueueSnapshot;

            var analyzerData = new List<DiagnosticAnalyzerData>
            {
                new DiagnosticAnalyzerDataImpl("ConsumerUtilization", data.ConsumerUtilization.ToString()),
                new DiagnosticAnalyzerDataImpl("Analyzer.ConsumerUtilizationWarningCoefficient", _config.ConsumerUtilizationWarningCoefficient.ToString())
            };

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            if (data.ConsumerUtilization >= _config.ConsumerUtilizationWarningCoefficient && data.ConsumerUtilization < 1.0M)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticAnalyzerResult(data.Node,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else if (data.ConsumerUtilization < _config.ConsumerUtilizationWarningCoefficient)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticAnalyzerResult(data.Node,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticAnalyzerResult(data.Node,
                    data.Identifier,
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
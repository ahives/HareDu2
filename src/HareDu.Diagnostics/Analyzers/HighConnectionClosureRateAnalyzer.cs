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
    using Configuration;
    using Core.Extensions;
    using Internal;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class HighConnectionClosureRateAnalyzer :
        BaseDiagnosticAnalyzer,
        IDiagnosticAnalyzer
    {
        public string Identifier => GetType().GenerateIdentifier();
        public string Description => "";
        public ComponentType ComponentType => ComponentType.Connection;
        public DiagnosticAnalyzerCategory Category => DiagnosticAnalyzerCategory.Connectivity;
        public DiagnosticAnalyzerStatus Status => _status;

        public HighConnectionClosureRateAnalyzer(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(configProvider, knowledgeBaseProvider)
        {
            _status = _configProvider.TryGet(out _config) ? DiagnosticAnalyzerStatus.Online : DiagnosticAnalyzerStatus.Offline;
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            DiagnosticResult result;
            BrokerConnectivitySnapshot data = snapshot as BrokerConnectivitySnapshot;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }

            var analyzerData = new List<DiagnosticAnalyzerData>
            {
                new DiagnosticAnalyzerDataImpl("ConnectionsClosed.Rate", data.ConnectionsClosed.Rate.ToString()),
                new DiagnosticAnalyzerDataImpl("HighClosureRateThreshold", _config.Sensor.HighClosureRateWarningThreshold.ToString())
            };

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            if (data.ConnectionsClosed.Rate >= _config.Sensor.HighClosureRateWarningThreshold)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticResult(null, Identifier, ComponentType, analyzerData, knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(null, Identifier, ComponentType, analyzerData, knowledgeBaseArticle);
            }

            NotifyObservers(result);

            return result;
        }
    }
}
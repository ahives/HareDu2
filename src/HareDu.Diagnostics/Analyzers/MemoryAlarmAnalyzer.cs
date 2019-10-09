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
    using Internal;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class MemoryAlarmAnalyzer :
        BaseDiagnosticAnalyzer,
        IDiagnosticAnalyzer
    {
        public string Identifier => GetType().GetIdentifier();
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Memory;
        public DiagnosticAnalyzerCategory Category => DiagnosticAnalyzerCategory.Throughput;
        public DiagnosticAnalyzerStatus Status => _status;

        public MemoryAlarmAnalyzer(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(configProvider, knowledgeBaseProvider)
        {
            _status = DiagnosticAnalyzerStatus.Online;
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            MemorySnapshot data = snapshot as MemorySnapshot;
            DiagnosticResult result;

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            var analyzerData = new List<DiagnosticAnalyzerData>
            {
                new DiagnosticAnalyzerDataImpl("Memory.FreeAlarm", data.AlarmInEffect.ToString()),
                new DiagnosticAnalyzerDataImpl("Memory.Limit", data.Limit.ToString()),
                new DiagnosticAnalyzerDataImpl("Memory.Used", data.Used.ToString())
            };

            if (data.AlarmInEffect)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticResult(data.NodeIdentifier,null, Identifier, ComponentType, analyzerData, knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(data.NodeIdentifier, null, Identifier, ComponentType, analyzerData, knowledgeBaseArticle);
            }

            NotifyObservers(result);
                
            return result;
        }
    }
}
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
namespace HareDu.Diagnostics.Analyzers
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Extensions;
    using Internal;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class QueueLowFlowAnalyzer :
        BaseDiagnosticAnalyzer,
        IDiagnosticAnalyzer
    {
        readonly DiagnosticAnalyzerConfig _config;
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Queue Low Flow Analyzer";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Queue;
        public DiagnosticAnalyzerCategory Category => DiagnosticAnalyzerCategory.Throughput;
        public DiagnosticAnalyzerStatus Status => _status;

        public QueueLowFlowAnalyzer(DiagnosticAnalyzerConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
            _config = config;
            _status = !_config.IsNull() ? DiagnosticAnalyzerStatus.Online : DiagnosticAnalyzerStatus.Offline;
        }

        public DiagnosticAnalyzerResult Execute<T>(T snapshot)
        {
            QueueSnapshot data = snapshot as QueueSnapshot;
            DiagnosticAnalyzerResult result;
            
            var analyzerData = new List<DiagnosticAnalyzerData>
            {
                new DiagnosticAnalyzerDataImpl("Messages.Incoming.Total", data.Messages.Incoming.Total.ToString()),
                new DiagnosticAnalyzerDataImpl("QueueLowFlowThreshold", _config.QueueLowFlowThreshold.ToString())
            };
            
            KnowledgeBaseArticle knowledgeBaseArticle;
            
            if (data.Messages.Incoming.Total <= _config.QueueLowFlowThreshold)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticAnalyzerResult(data.Node, data.Identifier, Identifier, ComponentType, analyzerData, knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticAnalyzerResult(data.Node, data.Identifier, Identifier, ComponentType, analyzerData, knowledgeBaseArticle);
            }

            NotifyObservers(result);

            return result;
        }
    }
}
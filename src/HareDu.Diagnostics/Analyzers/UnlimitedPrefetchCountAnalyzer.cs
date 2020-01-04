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

    public class UnlimitedPrefetchCountAnalyzer :
        BaseDiagnosticAnalyzer,
        IDiagnosticAnalyzer
    {
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Unlimited Prefetch Count Analyzer";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Channel;
        public DiagnosticAnalyzerCategory Category => DiagnosticAnalyzerCategory.Throughput;
        public DiagnosticAnalyzerStatus Status => _status;

        public UnlimitedPrefetchCountAnalyzer(DiagnosticAnalyzerConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
            _status = DiagnosticAnalyzerStatus.Online;
        }

        public DiagnosticAnalyzerResult Execute<T>(T snapshot)
        {
            ChannelSnapshot data = snapshot as ChannelSnapshot;
            DiagnosticAnalyzerResult result;
            
            var analyzerData = new List<DiagnosticAnalyzerData>
            {
                new DiagnosticAnalyzerDataImpl("PrefetchCount", data.PrefetchCount.ToString())
            };

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            if (data.PrefetchCount == 0)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticAnalyzerResult(data.ConnectionIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Inconclusive, out knowledgeBaseArticle);
                result = new InconclusiveDiagnosticAnalyzerResult(data.ConnectionIdentifier,
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
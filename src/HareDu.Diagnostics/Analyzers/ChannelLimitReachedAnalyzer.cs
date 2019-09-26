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
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Core.Extensions;
    using Internal;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class ChannelLimitReachedAnalyzer :
        BaseDiagnosticAnalyzer,
        IDiagnosticAnalyzer
    {
        public string Identifier => GetType().GenerateIdentifier();
        public string Description => "Measures actual number of channels to the defined limit on connection";
        public ComponentType ComponentType => ComponentType.Connection;
        public DiagnosticAnalyzerCategory Category => DiagnosticAnalyzerCategory.Throughput;
        public DiagnosticAnalyzerStatus Status => _status;

        public ChannelLimitReachedAnalyzer(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(configProvider, knowledgeBaseProvider)
        {
            _status = DiagnosticAnalyzerStatus.Online;
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            ConnectionSnapshot data = snapshot as ConnectionSnapshot;
            DiagnosticResult result;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }

            var analyzerData = new List<DiagnosticAnalyzerData>
            {
                new DiagnosticAnalyzerDataImpl("Channels.Count", data.Channels.Count.ToString()),
                new DiagnosticAnalyzerDataImpl("ChannelLimit", data.ChannelLimit.ToString())
            };

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            if (Convert.ToUInt64(data.Channels.Count) >= data.ChannelLimit)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticResult(data.NodeIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(data.NodeIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData, knowledgeBaseArticle);
            }

            NotifyObservers(result);

            return result;
        }
    }
}
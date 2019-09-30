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

    public class SocketDescriptorThrottlingAnalyzer :
        BaseDiagnosticAnalyzer,
        IDiagnosticAnalyzer
    {
        public string Identifier => GetType().GetIdentifier();
        public string Description =>
            "Checks network to see if the number of sockets currently in use is less than or equal to the number available.";
        public ComponentType ComponentType => ComponentType.Node;
        public DiagnosticAnalyzerCategory Category => DiagnosticAnalyzerCategory.Throughput;
        public DiagnosticAnalyzerStatus Status => _status;

        public SocketDescriptorThrottlingAnalyzer(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(configProvider, knowledgeBaseProvider)
        {
            _status = _configProvider.TryGet(out _config) ? DiagnosticAnalyzerStatus.Online : DiagnosticAnalyzerStatus.Offline;
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            DiagnosticResult result;
            NodeSnapshot data = snapshot as NodeSnapshot;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }

            KnowledgeBaseArticle knowledgeBaseArticle;
            ulong warningThreshold = ComputeWarningThreshold(data.OS.SocketDescriptors.Available);
            
            var analyzerData = new List<DiagnosticAnalyzerData>
            {
                new DiagnosticAnalyzerDataImpl("OS.Sockets.Available", data.OS.SocketDescriptors.Available.ToString()),
                new DiagnosticAnalyzerDataImpl("OS.Sockets.Used", data.OS.SocketDescriptors.Used.ToString()),
                new DiagnosticAnalyzerDataImpl("CalculatedWarningThreshold", warningThreshold.ToString())
            };

            if (data.OS.SocketDescriptors.Used < warningThreshold && warningThreshold < data.OS.SocketDescriptors.Available)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(data.ClusterIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else if (data.OS.SocketDescriptors.Used == data.OS.SocketDescriptors.Available)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticResult(data.ClusterIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticResult(data.ClusterIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }

            NotifyObservers(result);
                
            return result;
        }

        ulong ComputeWarningThreshold(ulong socketsAvailable)
            => _config.Analyzer.SocketUsageCoefficient >= 1
                ? socketsAvailable
                : Convert.ToUInt64(Math.Ceiling(socketsAvailable * _config.Analyzer.SocketUsageCoefficient));
    }
}
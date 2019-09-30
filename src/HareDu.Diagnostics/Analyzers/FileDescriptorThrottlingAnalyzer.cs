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

    public class FileDescriptorThrottlingAnalyzer :
        BaseDiagnosticAnalyzer,
        IDiagnosticAnalyzer
    {
        public string Identifier => GetType().GetIdentifier();
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Node;
        public DiagnosticAnalyzerCategory Category => DiagnosticAnalyzerCategory.Throughput;
        public DiagnosticAnalyzerStatus Status => _status;

        public FileDescriptorThrottlingAnalyzer(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(configProvider, knowledgeBaseProvider)
        {
            _status = _configProvider.TryGet(out _config) ? DiagnosticAnalyzerStatus.Online : DiagnosticAnalyzerStatus.Offline;
        }

        public DiagnosticResult Execute<T>(T snapshot)
        {
            DiagnosticResult result;
            OperatingSystemSnapshot data = snapshot as OperatingSystemSnapshot;
            
            if (data.IsNull())
            {
                result = new InconclusiveDiagnosticResult(null, null, Identifier, ComponentType);

                NotifyObservers(result);

                return result;
            }

            KnowledgeBaseArticle knowledgeBaseArticle;
            ulong warningThreshold = ComputeWarningThreshold(data.FileDescriptors.Available);

            var analyzerData = new List<DiagnosticAnalyzerData>
            {
                new DiagnosticAnalyzerDataImpl("FileDescriptors.Available", data.FileDescriptors.Available.ToString()),
                new DiagnosticAnalyzerDataImpl("FileDescriptors.Used", data.FileDescriptors.Used.ToString()),
                new DiagnosticAnalyzerDataImpl("FileDescriptorUsageWarningThreshold", _config.Analyzer.FileDescriptorUsageWarningCoefficient.ToString()),
                new DiagnosticAnalyzerDataImpl("CalculatedWarningThreshold", warningThreshold.ToString())
            };

            if (data.FileDescriptors.Used < warningThreshold && warningThreshold < data.FileDescriptors.Available)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticResult(data.NodeIdentifier,
                    data.ProcessId,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else if (data.FileDescriptors.Used == data.FileDescriptors.Available)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticResult(data.NodeIdentifier,
                    data.ProcessId,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticResult(data.NodeIdentifier,
                    data.ProcessId,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }

            NotifyObservers(result);

            return result;
        }

        ulong ComputeWarningThreshold(ulong fileDescriptorsAvailable)
            => _config.Analyzer.FileDescriptorUsageWarningCoefficient >= 1
                ? fileDescriptorsAvailable
                : Convert.ToUInt64(Math.Ceiling(fileDescriptorsAvailable * _config.Analyzer.FileDescriptorUsageWarningCoefficient));
    }
}
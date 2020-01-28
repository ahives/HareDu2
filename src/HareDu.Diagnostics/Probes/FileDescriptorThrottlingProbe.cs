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
    using System;
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Extensions;
    using Internal;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class FileDescriptorThrottlingProbe :
        BaseDiagnosticProbe,
        IDiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        public string Identifier => GetType().GetIdentifier();
        public string Name => "File Descriptor Throttling Analyzer";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.OperatingSystem;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.Throughput;
        public DiagnosticProbeStatus Status => _status;

        public FileDescriptorThrottlingProbe(DiagnosticsConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
            _config = config;
            _status = !_config.IsNull() ? DiagnosticProbeStatus.Online : DiagnosticProbeStatus.Offline;
        }

        public DiagnosticProbeResult Execute<T>(T snapshot)
        {
            DiagnosticProbeResult result;
            OperatingSystemSnapshot data = snapshot as OperatingSystemSnapshot;
            
            KnowledgeBaseArticle knowledgeBaseArticle;
            ulong warningThreshold = ComputeWarningThreshold(data.FileDescriptors.Available);

            var analyzerData = new List<DiagnosticProbeData>
            {
                new DiagnosticProbeDataImpl("FileDescriptors.Available", data.FileDescriptors.Available.ToString()),
                new DiagnosticProbeDataImpl("FileDescriptors.Used", data.FileDescriptors.Used.ToString()),
                new DiagnosticProbeDataImpl("FileDescriptorUsageWarningThreshold", _config.FileDescriptorUsageWarningCoefficient.ToString()),
                new DiagnosticProbeDataImpl("CalculatedWarningThreshold", warningThreshold.ToString())
            };

            if (data.FileDescriptors.Used < warningThreshold && warningThreshold < data.FileDescriptors.Available)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticProbeResult(data.NodeIdentifier,
                    data.ProcessId,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else if (data.FileDescriptors.Used == data.FileDescriptors.Available)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticProbeResult(data.NodeIdentifier,
                    data.ProcessId,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticProbeResult(data.NodeIdentifier,
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
            => _config.FileDescriptorUsageWarningCoefficient >= 1
                ? fileDescriptorsAvailable
                : Convert.ToUInt64(Math.Ceiling(fileDescriptorsAvailable * _config.FileDescriptorUsageWarningCoefficient));
    }
}
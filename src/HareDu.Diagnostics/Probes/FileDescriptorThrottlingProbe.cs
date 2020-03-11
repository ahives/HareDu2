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
    using KnowledgeBase;
    using Snapshotting.Model;

    public class FileDescriptorThrottlingProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        public string Identifier => GetType().GetIdentifier();
        public string Name => "File Descriptor Throttling Probe";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.OperatingSystem;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.Throughput;

        public FileDescriptorThrottlingProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            OperatingSystemSnapshot data = snapshot as OperatingSystemSnapshot;

            if (_config.IsNull())
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.NA, out var article);
                result = new NotApplicableProbeResult(!data.IsNull() ? data.NodeIdentifier : null,
                    !data.IsNull() ? data.ProcessId : null,
                    Identifier,
                    ComponentType,
                    article);

                NotifyObservers(result);

                return result;
            }
            
            ulong warningThreshold = ComputeWarningThreshold(data.FileDescriptors.Available);

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("FileDescriptors.Available", data.FileDescriptors.Available.ToString()),
                new ProbeDataImpl("FileDescriptors.Used", data.FileDescriptors.Used.ToString()),
                new ProbeDataImpl("FileDescriptorUsageThresholdCoefficient", _config.Probes.FileDescriptorUsageThresholdCoefficient.ToString()),
                new ProbeDataImpl("CalculatedThreshold", warningThreshold.ToString())
            };

            if (data.FileDescriptors.Used < warningThreshold && warningThreshold < data.FileDescriptors.Available)
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.NodeIdentifier,
                    data.ProcessId,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }
            else if (data.FileDescriptors.Used == data.FileDescriptors.Available)
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.NodeIdentifier,
                    data.ProcessId,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Warning, out var article);
                result = new WarningProbeResult(data.NodeIdentifier,
                    data.ProcessId,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }

            NotifyObservers(result);

            return result;
        }

        ulong ComputeWarningThreshold(ulong fileDescriptorsAvailable)
            => _config.Probes.FileDescriptorUsageThresholdCoefficient >= 1
                ? fileDescriptorsAvailable
                : Convert.ToUInt64(Math.Ceiling(fileDescriptorsAvailable * _config.Probes.FileDescriptorUsageThresholdCoefficient));
    }
}
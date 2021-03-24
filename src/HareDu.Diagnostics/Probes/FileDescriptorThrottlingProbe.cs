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
        
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<FileDescriptorThrottlingProbe>("File Descriptor Throttling Probe", "");
        public ComponentType ComponentType => ComponentType.OperatingSystem;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public FileDescriptorThrottlingProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            OperatingSystemSnapshot data = snapshot as OperatingSystemSnapshot;

            if (_config.IsNull() || _config.Probes.IsNull())
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.NA, out var article);
                result = new NotApplicableProbeResult(!data.IsNull() ? data.NodeIdentifier : null,
                    !data.IsNull() ? data.ProcessId : null,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    article);

                NotifyObservers(result);

                return result;
            }
            
            ulong threshold = ComputeThreshold(data.FileDescriptors.Available);

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("FileDescriptors.Available", data.FileDescriptors.Available.ToString()),
                new ProbeDataImpl("FileDescriptors.Used", data.FileDescriptors.Used.ToString()),
                new ProbeDataImpl("FileDescriptorUsageThresholdCoefficient", _config.Probes.FileDescriptorUsageThresholdCoefficient.ToString()),
                new ProbeDataImpl("CalculatedThreshold", threshold.ToString())
            };

            if (data.FileDescriptors.Used < threshold && threshold < data.FileDescriptors.Available)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.NodeIdentifier,
                    data.ProcessId,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    probeData,
                    article);
            }
            else if (data.FileDescriptors.Used == data.FileDescriptors.Available)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.NodeIdentifier,
                    data.ProcessId,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Warning, out var article);
                result = new WarningProbeResult(data.NodeIdentifier,
                    data.ProcessId,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    probeData,
                    article);
            }

            NotifyObservers(result);

            return result;
        }

        ulong ComputeThreshold(ulong fileDescriptorsAvailable)
            => _config.Probes.FileDescriptorUsageThresholdCoefficient >= 1
                ? fileDescriptorsAvailable
                : Convert.ToUInt64(Math.Ceiling(fileDescriptorsAvailable * _config.Probes.FileDescriptorUsageThresholdCoefficient));
    }
}
namespace HareDu.Diagnostics.Probes
{
    using System;
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class RuntimeProcessLimitProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<RuntimeProcessLimitProbe>("Runtime Process Limit Probe", "");
        public ComponentType ComponentType => ComponentType.Runtime;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public RuntimeProcessLimitProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            BrokerRuntimeSnapshot data = snapshot as BrokerRuntimeSnapshot;

            if (_config.IsNull() || _config.Probes.IsNull())
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.NA, out var article);
                result = new NotApplicableProbeResult(null,
                    null,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    article);

                NotifyObservers(result);

                return result;
            }

            ulong threshold = ComputeThreshold(data.Processes.Limit);

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Processes.Limit", data.Processes.Limit.ToString()),
                new ProbeDataImpl("Processes.Used", data.Processes.Used.ToString()),
                new ProbeDataImpl("CalculatedThreshold", threshold.ToString())
            };

            if (data.Processes.Used >= data.Processes.Limit)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.ClusterIdentifier,
                    data.Identifier,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    probeData,
                    article);
            }
            else if (data.Processes.Used >= threshold && threshold < data.Processes.Limit)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Healthy, out var article);
                result = new WarningProbeResult(data.ClusterIdentifier,
                    data.Identifier,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.ClusterIdentifier,
                    data.Identifier,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    probeData,
                    article);
            }

            NotifyObservers(result);

            return result;
        }

        ulong ComputeThreshold(ulong limit)
            => _config.Probes.RuntimeProcessUsageThresholdCoefficient >= 1
                ? limit
                : Convert.ToUInt64(Math.Ceiling(limit * _config.Probes.RuntimeProcessUsageThresholdCoefficient));
    }
}
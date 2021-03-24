namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class ConsumerUtilizationProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<ConsumerUtilizationProbe>("Consumer Utilization Probe", "");
        public ComponentType ComponentType => ComponentType.Queue;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public ConsumerUtilizationProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            QueueSnapshot data = snapshot as QueueSnapshot;

            if (_config.IsNull() || _config.Probes.IsNull())
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.NA, out var article);
                result = new NotApplicableProbeResult(!data.IsNull() ? data.Node : null,
                    !data.IsNull() ? data.Identifier : null,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    article);

                NotifyObservers(result);

                return result;
            }

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("ConsumerUtilization", data.ConsumerUtilization.ToString()),
                new ProbeDataImpl("ConsumerUtilizationThreshold", _config.Probes.ConsumerUtilizationThreshold.ToString())
            };
            
            if (data.ConsumerUtilization >= _config.Probes.ConsumerUtilizationThreshold
                && data.ConsumerUtilization < 1.0M
                && _config.Probes.ConsumerUtilizationThreshold <= 1.0M)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Warning, out var article);
                result = new WarningProbeResult(data.Node,
                    data.Identifier,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    probeData,
                    article);
            }
            else if (data.ConsumerUtilization < _config.Probes.ConsumerUtilizationThreshold
                     && _config.Probes.ConsumerUtilizationThreshold <= 1.0M)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.Node,
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
                result = new HealthyProbeResult(data.Node,
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
    }
}
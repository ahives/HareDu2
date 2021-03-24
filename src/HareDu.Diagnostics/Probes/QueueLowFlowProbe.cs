namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class QueueLowFlowProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<QueueLowFlowProbe>("Queue Low Flow Probe", "");
        public ComponentType ComponentType => ComponentType.Queue;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public QueueLowFlowProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            QueueSnapshot data = snapshot as QueueSnapshot;
            ProbeResult result;
 
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
           
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Messages.Incoming.Total", data.Messages.Incoming.Total.ToString()),
                new ProbeDataImpl("QueueLowFlowThreshold", _config.Probes.QueueLowFlowThreshold.ToString())
            };
            
            if (data.Messages.Incoming.Total <= _config.Probes.QueueLowFlowThreshold)
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
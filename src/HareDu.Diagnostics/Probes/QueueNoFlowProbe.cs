namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class QueueNoFlowProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<QueueNoFlowProbe>("Queue No Flow Probe", "");
        public ComponentType ComponentType => ComponentType.Queue;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public QueueNoFlowProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            QueueSnapshot data = snapshot as QueueSnapshot;
            ProbeResult result;
            
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Messages.Incoming.Total", data.Messages.Incoming.Total.ToString())
            };
            
            if (data.Messages.Incoming.Total == 0)
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
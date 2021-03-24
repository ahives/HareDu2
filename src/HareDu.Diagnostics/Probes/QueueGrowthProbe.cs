namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class QueueGrowthProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<QueueGrowthProbe>("Queue Growth Probe", "");
        public ComponentType ComponentType => ComponentType.Queue;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public QueueGrowthProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            QueueSnapshot data = snapshot as QueueSnapshot;
            ProbeResult result;
            
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Messages.Incoming.Rate", data.Messages.Incoming.Rate.ToString()),
                new ProbeDataImpl("Messages.Acknowledged.Rate", data.Messages.Acknowledged.Rate.ToString())
            };
            
            if (data.Messages.Incoming.Rate > data.Messages.Acknowledged.Rate)
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
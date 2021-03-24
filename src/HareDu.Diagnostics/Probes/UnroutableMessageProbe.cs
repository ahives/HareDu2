namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class UnroutableMessageProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<UnroutableMessageProbe>("Unroutable Message Probe", "");
        public ComponentType ComponentType => ComponentType.Exchange;
        public ProbeCategory Category => ProbeCategory.Efficiency;

        public UnroutableMessageProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            BrokerQueuesSnapshot data = snapshot as BrokerQueuesSnapshot;
            
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Churn.NotRouted.Total", data.Churn.NotRouted.Total.ToString())
            };

            if (data.Churn.NotRouted.Total > 0)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.ClusterName,
                    null,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.ClusterName,
                    null,
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
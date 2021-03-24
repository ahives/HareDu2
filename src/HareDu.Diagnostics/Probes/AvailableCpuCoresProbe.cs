namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class AvailableCpuCoresProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<AvailableCpuCoresProbe>("Available CPU Cores Probe", "");
        public ComponentType ComponentType => ComponentType.Node;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public AvailableCpuCoresProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            NodeSnapshot data = snapshot as NodeSnapshot;

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("AvailableCoresDetected", data.AvailableCoresDetected.ToString())
            };
            
            if (data.AvailableCoresDetected <= 0)
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
    }
}
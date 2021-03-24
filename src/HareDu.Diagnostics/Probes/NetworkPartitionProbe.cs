namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class NetworkPartitionProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<NetworkPartitionProbe>("Network Partition Probe", "");
        public ComponentType ComponentType => ComponentType.Node;
        public ProbeCategory Category => ProbeCategory.Connectivity;

        public NetworkPartitionProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            NodeSnapshot data = snapshot as NodeSnapshot;
            ProbeResult result;

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("NetworkPartitions", data.NetworkPartitions.ToString())
            };
            
            if (data.NetworkPartitions.Any())
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
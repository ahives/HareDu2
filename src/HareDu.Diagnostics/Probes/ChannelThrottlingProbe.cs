namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class ChannelThrottlingProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<ChannelThrottlingProbe>("Channel Throttling Probe", "Monitors connections to the RabbitMQ broker to determine whether channels are being throttled.");
        public ComponentType ComponentType => ComponentType.Channel;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public ChannelThrottlingProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ChannelSnapshot data = snapshot as ChannelSnapshot;
            ProbeResult result;
            
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("UnacknowledgedMessages", data.UnacknowledgedMessages.ToString()),
                new ProbeDataImpl("PrefetchCount", data.PrefetchCount.ToString())
            };
            
            if (data.UnacknowledgedMessages > data.PrefetchCount)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.ConnectionIdentifier,
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
                result = new HealthyProbeResult(data.ConnectionIdentifier,
                    data.Identifier,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    probeData, article);
            }

            NotifyObservers(result);
                
            return result;
        }
    }
}
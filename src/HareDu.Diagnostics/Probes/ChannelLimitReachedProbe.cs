namespace HareDu.Diagnostics.Probes
{
    using System;
    using System.Collections.Generic;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class ChannelLimitReachedProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<ChannelLimitReachedProbe>("Channel Limit Reached Probe", "Measures actual number of channels to the defined limit on connection");
        public ComponentType ComponentType => ComponentType.Connection;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public ChannelLimitReachedProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ConnectionSnapshot data = snapshot as ConnectionSnapshot;
            ProbeResult result;

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Channels.Count", data.Channels.Count.ToString()),
                new ProbeDataImpl("OpenChannelLimit", data.OpenChannelsLimit.ToString())
            };
            
            if (Convert.ToUInt64(data.Channels.Count) >= data.OpenChannelsLimit)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.NodeIdentifier,
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
                result = new HealthyProbeResult(data.NodeIdentifier,
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
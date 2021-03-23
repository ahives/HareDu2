namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using Core.Extensions;
    using KnowledgeBase;
    using Model;
    using Snapshotting.Model;

    public class BlockedConnectionProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public string Id => GetType().GetIdentifier();
        public string Name => "Blocked Connection Probe";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Connection;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public BlockedConnectionProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            ConnectionSnapshot data = snapshot as ConnectionSnapshot;

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("State", data.State.ToString())
            };
            
            if (data.State == BrokerConnectionState.Blocked)
            {
                _kb.TryGet(Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.NodeIdentifier,
                    data.Identifier,
                    Id,
                    Name,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Id, ProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.NodeIdentifier,
                    data.Identifier,
                    Id,
                    Name,
                    ComponentType,
                    probeData,
                    article);
            }

            NotifyObservers(result);

            return result;
        }
    }
}
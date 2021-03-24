namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class MemoryAlarmProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<MemoryAlarmProbe>("Memory Alarm Probe", "");
        public ComponentType ComponentType => ComponentType.Memory;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public MemoryAlarmProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            MemorySnapshot data = snapshot as MemorySnapshot;
            ProbeResult result;

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Memory.FreeAlarm", data.AlarmInEffect.ToString()),
                new ProbeDataImpl("Memory.Limit", data.Limit.ToString()),
                new ProbeDataImpl("Memory.Used", data.Used.ToString())
            };
            
            if (data.AlarmInEffect)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.NodeIdentifier,
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
                result = new HealthyProbeResult(data.NodeIdentifier,
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
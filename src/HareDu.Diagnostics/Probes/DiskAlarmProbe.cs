namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class DiskAlarmProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<DiskAlarmProbe>("Disk Alarm Probe", "");
        public ComponentType ComponentType => ComponentType.Disk;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public DiskAlarmProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            DiskSnapshot data = snapshot as DiskSnapshot;
            ProbeResult result;

            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Disk.FreeAlarm", data.AlarmInEffect.ToString()),
                new ProbeDataImpl("Disk.Limit", data.Limit.ToString()),
                new ProbeDataImpl("Disk.Capacity.Available", data.Capacity.Available.ToString())
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
namespace HareDu.Diagnostics.Probes
{
    using System;
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class SocketDescriptorThrottlingProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<SocketDescriptorThrottlingProbe>("Socket Descriptor Throttling Probe", "Checks network to see if the number of sockets currently in use is less than or equal to the number available.");
        public ComponentType ComponentType => ComponentType.Node;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public SocketDescriptorThrottlingProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            NodeSnapshot data = snapshot as NodeSnapshot;

            if (_config.IsNull() || _config.Probes.IsNull())
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.NA, out var article);
                result = new NotApplicableProbeResult(!data.IsNull() ? data.ClusterIdentifier : null,
                    !data.IsNull() ? data.Identifier : null,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    article);

                NotifyObservers(result);

                return result;
            }

            ulong warningThreshold = ComputeThreshold(data.OS.SocketDescriptors.Available);
            
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("OS.Sockets.Available", data.OS.SocketDescriptors.Available.ToString()),
                new ProbeDataImpl("OS.Sockets.Used", data.OS.SocketDescriptors.Used.ToString()),
                new ProbeDataImpl("CalculatedThreshold", warningThreshold.ToString())
            };

            if (data.OS.SocketDescriptors.Used < warningThreshold && warningThreshold < data.OS.SocketDescriptors.Available)
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
            else if (data.OS.SocketDescriptors.Used == data.OS.SocketDescriptors.Available)
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
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Warning, out var article);
                result = new WarningProbeResult(data.ClusterIdentifier,
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

        ulong ComputeThreshold(ulong socketsAvailable)
            => _config.Probes.SocketUsageThresholdCoefficient >= 1
                ? socketsAvailable
                : Convert.ToUInt64(Math.Ceiling(socketsAvailable * _config.Probes.SocketUsageThresholdCoefficient));
    }
}
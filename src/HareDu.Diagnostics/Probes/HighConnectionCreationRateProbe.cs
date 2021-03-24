namespace HareDu.Diagnostics.Probes
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class HighConnectionCreationRateProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<HighConnectionCreationRateProbe>("High Connection Creation Rate Probe", "");
        public ComponentType ComponentType => ComponentType.Connection;
        public ProbeCategory Category => ProbeCategory.Connectivity;

        public HighConnectionCreationRateProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            BrokerConnectivitySnapshot data = snapshot as BrokerConnectivitySnapshot;

            if (_config.IsNull() || _config.Probes.IsNull())
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.NA, out var article);
                result = new NotApplicableProbeResult(null,
                    null,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    article);

                NotifyObservers(result);

                return result;
            }
            
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("ConnectionsCreated.Rate", data.ConnectionsCreated.Rate.ToString()),
                new ProbeDataImpl("HighConnectionCreationRateThreshold", _config.Probes.HighConnectionCreationRateThreshold.ToString())
            };
            
            if (data.ConnectionsCreated.Rate >= _config.Probes.HighConnectionCreationRateThreshold)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Warning, out var article);
                result = new WarningProbeResult(null,
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
                result = new HealthyProbeResult(null,
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
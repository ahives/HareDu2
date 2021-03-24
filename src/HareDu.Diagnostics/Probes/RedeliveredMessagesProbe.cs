namespace HareDu.Diagnostics.Probes
{
    using System;
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class RedeliveredMessagesProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        
        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<RedeliveredMessagesProbe>("Redelivered Messages Probe", "");
        public ComponentType ComponentType => ComponentType.Queue;
        public ProbeCategory Category => ProbeCategory.FaultTolerance;

        public RedeliveredMessagesProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            QueueSnapshot data = snapshot as QueueSnapshot;
            
            if (_config.IsNull() || _config.Probes.IsNull())
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.NA, out var article);
                result = new NotApplicableProbeResult(!data.IsNull() ? data.Node : string.Empty,
                    !data.IsNull() ? data.Identifier : string.Empty,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    article);

                NotifyObservers(result);

                return result;
            }
            
            ulong warningThreshold = ComputeThreshold(data.Messages.Incoming.Total);
            
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Messages.Incoming.Total", data.Messages.Incoming.Total.ToString()),
                new ProbeDataImpl("Messages.Redelivered.Total", data.Messages.Redelivered.Total.ToString()),
                new ProbeDataImpl("MessageRedeliveryThresholdCoefficient", _config.Probes.MessageRedeliveryThresholdCoefficient.ToString()),
                new ProbeDataImpl("CalculatedThreshold", warningThreshold.ToString())
            };
            
            if (data.Messages.Redelivered.Total >= warningThreshold
                && data.Messages.Redelivered.Total < data.Messages.Incoming.Total
                && warningThreshold < data.Messages.Incoming.Total)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Warning, out var article);
                result = new WarningProbeResult(data.Node,
                    data.Identifier,
                    Metadata.Id,
                    Metadata.Name,
                    ComponentType,
                    probeData,
                    article);
            }
            else if (data.Messages.Redelivered.Total >= data.Messages.Incoming.Total)
            {
                _kb.TryGet(Metadata.Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.Node,
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
                result = new HealthyProbeResult(data.Node,
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

        ulong ComputeThreshold(ulong total)
            => _config.Probes.MessageRedeliveryThresholdCoefficient >= 1
                ? total
                : Convert.ToUInt64(Math.Ceiling(total * _config.Probes.MessageRedeliveryThresholdCoefficient));
    }
}
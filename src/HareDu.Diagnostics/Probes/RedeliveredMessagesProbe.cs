// Copyright 2013-2020 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Redelivered Messages Probe";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Queue;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.FaultTolerance;
        public ProbeStatus Status => _status;

        public RedeliveredMessagesProbe(DiagnosticsConfig config, IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _config = config;
            _status = !_config.IsNull() ? ProbeStatus.Online : ProbeStatus.Offline;
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ProbeResult result;
            QueueSnapshot data = snapshot as QueueSnapshot;
            
            ulong warningThreshold = ComputeWarningThreshold(data.Messages.Incoming.Total);
            
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
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Warning, out var article);
                result = new WarningProbeResult(data.Node,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }
            else if (data.Messages.Redelivered.Total >= data.Messages.Incoming.Total)
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.Node,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.Node,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }

            NotifyObservers(result);

            return result;
        }

        ulong ComputeWarningThreshold(ulong total)
            => _config.Probes.MessageRedeliveryThresholdCoefficient >= 1
                ? total
                : Convert.ToUInt64(Math.Ceiling(total * _config.Probes.MessageRedeliveryThresholdCoefficient));
    }
}
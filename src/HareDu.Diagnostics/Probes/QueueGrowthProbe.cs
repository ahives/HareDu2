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
    using System.Collections.Generic;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class QueueGrowthProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Queue Growth Probe";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Queue;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.Throughput;

        public QueueGrowthProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            QueueSnapshot data = snapshot as QueueSnapshot;
            ProbeResult result;
            
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("Messages.Incoming.Rate", data.Messages.Incoming.Rate.ToString()),
                new ProbeDataImpl("Messages.Acknowledged.Rate", data.Messages.Acknowledged.Rate.ToString())
            };
            
            if (data.Messages.Incoming.Rate > data.Messages.Acknowledged.Rate)
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Warning, out var article);
                result = new WarningProbeResult(data.Node,
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
    }
}
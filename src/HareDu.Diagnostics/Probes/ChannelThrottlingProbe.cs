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

    public class ChannelThrottlingProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public string Id => GetType().GetIdentifier();
        public string Name => "Channel Throttling Probe";
        public string Description => "Monitors connections to the RabbitMQ broker to determine whether channels are being throttled.";
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
                _kb.TryGet(Id, ProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.ConnectionIdentifier,
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
                result = new HealthyProbeResult(data.ConnectionIdentifier,
                    data.Identifier,
                    Id,
                    Name,
                    ComponentType,
                    probeData, article);
            }

            NotifyObservers(result);
                
            return result;
        }
    }
}
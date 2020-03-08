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
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class ChannelLimitReachedProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Channel Limit Reached Probe";
        public string Description => "Measures actual number of channels to the defined limit on connection";
        public ComponentType ComponentType => ComponentType.Connection;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.Throughput;
        public ProbeStatus Status => _status;

        public ChannelLimitReachedProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
            _status = ProbeStatus.Online;
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
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Unhealthy, out var article);
                result = new UnhealthyProbeResult(data.NodeIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Identifier, DiagnosticProbeResultStatus.Healthy, out var article);
                result = new HealthyProbeResult(data.NodeIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    probeData, article);
            }

            NotifyObservers(result);

            return result;
        }
    }
}
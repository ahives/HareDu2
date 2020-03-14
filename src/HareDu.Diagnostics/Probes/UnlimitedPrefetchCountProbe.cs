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
    using Core.Configuration;
    using Core.Extensions;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class UnlimitedPrefetchCountProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public string Id => GetType().GetIdentifier();
        public string Name => "Unlimited Prefetch Count Probe";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Channel;
        public ProbeCategory Category => ProbeCategory.Throughput;

        public UnlimitedPrefetchCountProbe(IKnowledgeBaseProvider kb)
            : base(kb)
        {
        }

        public ProbeResult Execute<T>(T snapshot)
        {
            ChannelSnapshot data = snapshot as ChannelSnapshot;
            ProbeResult result;
            
            var probeData = new List<ProbeData>
            {
                new ProbeDataImpl("PrefetchCount", data.PrefetchCount.ToString())
            };

            if (data.PrefetchCount == 0)
            {
                _kb.TryGet(Id, ProbeResultStatus.Warning, out var article);
                result = new WarningProbeResult(data.ConnectionIdentifier,
                    data.Identifier,
                    Id,
                    Name,
                    ComponentType,
                    probeData,
                    article);
            }
            else
            {
                _kb.TryGet(Id, ProbeResultStatus.Inconclusive, out var article);
                result = new InconclusiveProbeResult(data.ConnectionIdentifier,
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
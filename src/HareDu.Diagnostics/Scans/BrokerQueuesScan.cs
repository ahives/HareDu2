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
namespace HareDu.Diagnostics.Scans
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Probes;
    using Snapshotting.Model;

    public class BrokerQueuesScan :
        DiagnosticScan<BrokerQueuesSnapshot>
    {
        public string Identifier => GetType().GetIdentifier();

        readonly IReadOnlyList<DiagnosticProbe> _queueProbes;
        readonly List<DiagnosticProbe> _exchangeProbes;

        public BrokerQueuesScan(IReadOnlyList<DiagnosticProbe> probes)
        {
            if (probes.IsNull())
                throw new ArgumentNullException(nameof(probes));
            
            _queueProbes = probes.Where(IsQueueProbe).ToList();
            _exchangeProbes = probes.Where(IsExchangeProbe).ToList();
        }

        public IReadOnlyList<ProbeResult> Scan(BrokerQueuesSnapshot snapshot)
        {
            if (snapshot == null)
                return DiagnosticCache.EmptyProbeResults;
            
            var results = new List<ProbeResult>();

            results.AddRange(_exchangeProbes.Select(x => x.Execute(snapshot)));
            
            for (int i = 0; i < snapshot.Queues.Count; i++)
            {
                if (!snapshot.Queues[i].IsNull())
                    results.AddRange(_queueProbes.Select(x => x.Execute(snapshot.Queues[i])));
            }

            return results;
        }

        bool IsExchangeProbe(DiagnosticProbe probe) =>
            !probe.IsNull()
            && probe.Status == ProbeStatus.Online
            && probe.ComponentType == ComponentType.Exchange;

        bool IsQueueProbe(DiagnosticProbe probe) =>
            !probe.IsNull()
            && probe.Status == ProbeStatus.Online
            && probe.ComponentType == ComponentType.Queue;
    }
}
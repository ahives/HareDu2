namespace HareDu.Diagnostics.Scanners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Probes;
    using Snapshotting.Model;

    public class BrokerQueuesScanner :
        DiagnosticScanner<BrokerQueuesSnapshot>
    {
        public string Identifier => GetType().GetIdentifier();

        readonly IReadOnlyList<DiagnosticProbe> _queueProbes;
        readonly List<DiagnosticProbe> _exchangeProbes;

        public BrokerQueuesScanner(IReadOnlyList<DiagnosticProbe> probes)
        {
            if (probes.IsNull())
                throw new ArgumentNullException(nameof(probes));
            
            _queueProbes = probes
                .Where(x => !x.IsNull() && x.ComponentType == ComponentType.Queue)
                .ToList();
            _exchangeProbes = probes
                .Where(x => !x.IsNull() && x.ComponentType == ComponentType.Exchange)
                .ToList();
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
    }
}
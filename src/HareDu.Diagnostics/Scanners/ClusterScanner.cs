namespace HareDu.Diagnostics.Scanners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Probes;
    using Snapshotting.Model;

    public class ClusterScanner :
        BaseDiagnosticScanner,
        DiagnosticScanner<ClusterSnapshot>
    {
        IReadOnlyList<DiagnosticProbe> _nodeProbes;
        IReadOnlyList<DiagnosticProbe> _diskProbes;
        IReadOnlyList<DiagnosticProbe> _memoryProbes;
        IReadOnlyList<DiagnosticProbe> _runtimeProbes;
        IReadOnlyList<DiagnosticProbe> _osProbes;

        public DiagnosticScannerMetadata Metadata => new DiagnosticScannerMetadataImpl(GetType().GetIdentifier());

        public ClusterScanner(IReadOnlyList<DiagnosticProbe> probes)
        {
            Configure(probes.IsNotNull() ? probes : throw new ArgumentNullException(nameof(probes)));
        }

        public void Configure(IReadOnlyList<DiagnosticProbe> probes)
        {
            _nodeProbes = probes
                .Where(x => x.IsNotNull() && x.ComponentType == ComponentType.Node)
                .ToList();
            _diskProbes = probes
                .Where(x => x.IsNotNull() && x.ComponentType == ComponentType.Disk)
                .ToList();
            _memoryProbes = probes
                .Where(x => x.IsNotNull() && x.ComponentType == ComponentType.Memory)
                .ToList();
            _runtimeProbes = probes
                .Where(x => x.IsNotNull() && x.ComponentType == ComponentType.Runtime)
                .ToList();
            _osProbes = probes
                .Where(x => x.IsNotNull() && x.ComponentType == ComponentType.OperatingSystem)
                .ToList();
        }

        public IReadOnlyList<ProbeResult> Scan(ClusterSnapshot snapshot)
        {
            if (snapshot == null)
                return DiagnosticCache.EmptyProbeResults;
            
            var results = new List<ProbeResult>();

            for (int i = 0; i < snapshot.Nodes.Count; i++)
            {
                if (snapshot.Nodes[i].IsNull())
                    continue;
                
                results.AddRange(_nodeProbes.Select(x => x.Execute(snapshot.Nodes[i])));

                if (!snapshot.Nodes[i].Disk.IsNull())
                    results.AddRange(_diskProbes.Select(x => x.Execute(snapshot.Nodes[i].Disk)));

                if (!snapshot.Nodes[i].Memory.IsNull())
                    results.AddRange(_memoryProbes.Select(x => x.Execute(snapshot.Nodes[i].Memory)));

                if (!snapshot.Nodes[i].Runtime.IsNull())
                    results.AddRange(_runtimeProbes.Select(x => x.Execute(snapshot.Nodes[i].Runtime)));

                if (!snapshot.Nodes[i].OS.IsNull())
                    results.AddRange(_osProbes.Select(x => x.Execute(snapshot.Nodes[i].OS)));
            }

            return results;
        }
    }
}
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

    public class ClusterScanner :
        DiagnosticScan<ClusterSnapshot>
    {
        public string Identifier => GetType().GetIdentifier();

        readonly IReadOnlyList<DiagnosticProbe> _nodeProbes;
        readonly IReadOnlyList<DiagnosticProbe> _diskProbes;
        readonly IReadOnlyList<DiagnosticProbe> _memoryProbes;
        readonly IReadOnlyList<DiagnosticProbe> _runtimeProbes;
        readonly IReadOnlyList<DiagnosticProbe> _osProbes;

        public ClusterScanner(IReadOnlyList<DiagnosticProbe> probes)
        {
            if (probes.IsNull())
                throw new ArgumentNullException(nameof(probes));
            
            _nodeProbes = probes
                .Where(x => !x.IsNull() && x.ComponentType == ComponentType.Node)
                .ToList();
            _diskProbes = probes
                .Where(x => !x.IsNull() && x.ComponentType == ComponentType.Disk)
                .ToList();
            _memoryProbes = probes
                .Where(x => !x.IsNull() && x.ComponentType == ComponentType.Memory)
                .ToList();
            _runtimeProbes = probes
                .Where(x => !x.IsNull() && x.ComponentType == ComponentType.Runtime)
                .ToList();
            _osProbes = probes
                .Where(x => !x.IsNull() && x.ComponentType == ComponentType.OperatingSystem)
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
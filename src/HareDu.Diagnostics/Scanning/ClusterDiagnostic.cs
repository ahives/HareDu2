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
namespace HareDu.Diagnostics.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Extensions;
    using Probes;
    using Snapshotting.Model;

    public class ClusterDiagnostic :
        IComponentDiagnostic<ClusterSnapshot>
    {
        public string Identifier => GetType().GetIdentifier();

        readonly IReadOnlyList<IDiagnosticProbe> _nodeProbes;
        readonly IReadOnlyList<IDiagnosticProbe> _diskProbes;
        readonly IReadOnlyList<IDiagnosticProbe> _memoryProbes;
        readonly IReadOnlyList<IDiagnosticProbe> _runtimeProbes;
        readonly IReadOnlyList<IDiagnosticProbe> _operatingSystemProbes;

        public ClusterDiagnostic(IReadOnlyList<IDiagnosticProbe> probes)
        {
            if (probes.IsNull())
                throw new ArgumentNullException(nameof(probes));
            
            _nodeProbes = probes.Where(IsNodeProbe).ToList();
            _diskProbes = probes.Where(IsDiskProbe).ToList();
            _memoryProbes = probes.Where(IsMemoryProbe).ToList();
            _runtimeProbes = probes.Where(IsRuntimeProbe).ToList();
            _operatingSystemProbes = probes.Where(IsOperatingSystemProbe).ToList();
        }

        public IReadOnlyList<DiagnosticProbeResult> Scan(ClusterSnapshot snapshot)
        {
            if (snapshot == null)
                return DiagnosticCache.EmptyProbeResults;
            
            var results = new List<DiagnosticProbeResult>();

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
                    results.AddRange(_operatingSystemProbes.Select(x => x.Execute(snapshot.Nodes[i].OS)));
            }

            return results;
        }

        bool IsOperatingSystemProbe(IDiagnosticProbe probe) =>
            !probe.IsNull()
            && probe.Status == DiagnosticProbeStatus.Online
            && probe.ComponentType == ComponentType.OperatingSystem;

        bool IsRuntimeProbe(IDiagnosticProbe probe) =>
            !probe.IsNull()
            && probe.Status == DiagnosticProbeStatus.Online
            && probe.ComponentType == ComponentType.Runtime;

        bool IsMemoryProbe(IDiagnosticProbe probe) =>
            !probe.IsNull()
            && probe.Status == DiagnosticProbeStatus.Online
            && probe.ComponentType == ComponentType.Memory;

        bool IsDiskProbe(IDiagnosticProbe probe) =>
            !probe.IsNull()
            && probe.Status == DiagnosticProbeStatus.Online
            && probe.ComponentType == ComponentType.Disk;

        bool IsNodeProbe(IDiagnosticProbe probe) =>
            !probe.IsNull()
            && probe.Status == DiagnosticProbeStatus.Online
            && probe.ComponentType == ComponentType.Node;
    }
}
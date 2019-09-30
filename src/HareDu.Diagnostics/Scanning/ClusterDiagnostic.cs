// Copyright 2013-2019 Albert L. Hives
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
    using System.Collections.Generic;
    using System.Linq;
    using Analyzers;
    using Core.Extensions;
    using Snapshotting.Model;

    public class ClusterDiagnostic :
        IComponentDiagnostic<ClusterSnapshot>
    {
        public string Identifier => GetType().GetIdentifier();

        readonly IReadOnlyList<IDiagnosticAnalyzer> _nodeAnalyzers;
        readonly IReadOnlyList<IDiagnosticAnalyzer> _diskAnalyzers;
        readonly IReadOnlyList<IDiagnosticAnalyzer> _memoryAnalyzers;
        readonly IReadOnlyList<IDiagnosticAnalyzer> _runtimeAnalyzers;

        public ClusterDiagnostic(IReadOnlyList<IDiagnosticAnalyzer> analyzers)
        {
            _nodeAnalyzers = analyzers.Where(IsNodeAnalyzer).ToList();
            _diskAnalyzers = analyzers.Where(IsDiskAnalyzer).ToList();
            _memoryAnalyzers = analyzers.Where(IsMemoryAnalyzer).ToList();
            _runtimeAnalyzers = analyzers.Where(IsRuntimeAnalyzer).ToList();
        }

        public IReadOnlyList<DiagnosticResult> Scan(ClusterSnapshot snapshot)
        {
            if (snapshot == null)
                return DiagnosticCache.EmptyDiagnosticResults;
            
            var results = new List<DiagnosticResult>();

            for (int i = 0; i < snapshot.Nodes.Count; i++)
            {
                results.AddRange(_nodeAnalyzers.Select(x => x.Execute(snapshot.Nodes[i])));
                results.AddRange(_diskAnalyzers.Select(x => x.Execute(snapshot.Nodes[i].Disk)));
                results.AddRange(_memoryAnalyzers.Select(x => x.Execute(snapshot.Nodes[i].Memory)));
                results.AddRange(_runtimeAnalyzers.Select(x => x.Execute(snapshot.Nodes[i].Runtime)));
            }

            return results;
        }

        bool IsRuntimeAnalyzer(IDiagnosticAnalyzer analyzer) =>
            !analyzer.IsNull() &&
            analyzer.Status == DiagnosticAnalyzerStatus.Online
            && analyzer.ComponentType == ComponentType.Runtime;

        bool IsMemoryAnalyzer(IDiagnosticAnalyzer analyzer) =>
            !analyzer.IsNull()
            && analyzer.Status == DiagnosticAnalyzerStatus.Online
            && analyzer.ComponentType == ComponentType.Memory;

        bool IsDiskAnalyzer(IDiagnosticAnalyzer analyzer) =>
            !analyzer.IsNull()
            && analyzer.Status == DiagnosticAnalyzerStatus.Online
            && analyzer.ComponentType == ComponentType.Disk;

        bool IsNodeAnalyzer(IDiagnosticAnalyzer analyzer) =>
            !analyzer.IsNull()
            && analyzer.Status == DiagnosticAnalyzerStatus.Online
            && analyzer.ComponentType == ComponentType.Node;
    }
}
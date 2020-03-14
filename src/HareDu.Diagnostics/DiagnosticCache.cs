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
namespace HareDu.Diagnostics
{
    using System.Collections.Generic;
    using Analyzers;

    public static class DiagnosticCache
    {
        public static readonly IReadOnlyList<ProbeResult> EmptyProbeResults = new List<ProbeResult>();
        public static readonly ScannerResult EmptyScannerResult = new EmptyScannerResult();
        public static readonly IReadOnlyList<ProbeData> EmptyProbeData = new List<ProbeData>();
        public static readonly IReadOnlyList<AnalyzerSummary> EmptyAnalyzerSummary = new List<AnalyzerSummary>();
        public static readonly IScanAnalyzer NoOpAnalyzer = new NoOpScanAnalyzer();
    }
}
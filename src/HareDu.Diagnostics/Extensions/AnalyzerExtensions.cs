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
namespace HareDu.Diagnostics.Extensions
{
    using System;
    using System.Collections.Generic;
    using Core.Extensions;

    public static class AnalyzerExtensions
    {
        public static IReadOnlyList<AnalyzerSummary> Analyze(this ScannerResult report, IScannerResultAnalyzer analyzer, Func<ProbeResult, string> aggregationKey)
            => !analyzer.IsNull()
                ? analyzer.Analyze(report, aggregationKey)
                : DiagnosticCache.EmptyAnalyzerSummary;
    }
}
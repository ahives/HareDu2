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
namespace HareDu.Analytics
{
    using System;
    using System.Collections.Generic;
    using Diagnostics;

    public static class AnalyzerExtensions
    {
        public static IReadOnlyList<AnalyzerSummary> Analyze(this ScannerResult report, IDiagnosticReportAnalyzerFactory factory, string identifier)
            => factory.TryGet(identifier, out var analyzer)
                ? analyzer.Analyze(report)
                : AnalyticsCache.EmptyAnalyzerSummary;

        public static IReadOnlyList<AnalyzerSummary> Analyze(this ScannerResult report, IDiagnosticReportAnalyzer analyzer)
        {
            var summary = new List<AnalyzerSummary>();
            
            summary.AddRange(analyzer.Analyze(report));

            return summary;
        }

        public static IReadOnlyList<AnalyzerSummary> Analyze(this ScannerResult report, IDiagnosticReportAnalyzerFactory factory, Type type)
            => factory.TryGet(type, out var analyzer)
                ? analyzer.Analyze(report)
                : AnalyticsCache.EmptyAnalyzerSummary;

        public static IReadOnlyList<AnalyzerSummary> Analyze<T>(this ScannerResult report, IDiagnosticReportAnalyzerFactory factory)
            where T : IDiagnosticReportAnalyzer
            => factory.TryGet<T>(out var analyzer)
                ? analyzer.Analyze(report)
                : AnalyticsCache.EmptyAnalyzerSummary;
    }
}
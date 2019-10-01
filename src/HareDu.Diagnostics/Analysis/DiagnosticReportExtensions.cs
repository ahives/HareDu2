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
namespace HareDu.Diagnostics.Analysis
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DiagnosticReportExtensions
    {
        public static IReadOnlyList<AnalyzerSummary> Analyze(this DiagnosticReport report,
            IDiagnosticReportAnalyzerFactory factory)
        {
            var results = report.Results
                .Select(x => x.AnalyzerIdentifier)
                .Distinct(new IdentifierComparer())
                .ToList();

            var summary = new List<AnalyzerSummary>();
            
            for (int i = 0; i < results.Count; i++)
            {
                if (factory.TryGet(results[i], out var analyzer))
                    summary.AddRange(analyzer.Analyze(report));
            }

            return summary;
        }


        class IdentifierComparer :
            IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
                => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y) && x == y;

            public int GetHashCode(string obj) => obj.GetHashCode();
        }
    }
}
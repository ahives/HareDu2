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
namespace HareDu.Diagnostics.Analyzers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;

    public abstract class BaseAnalyzeDiagnosticReport
    {
        protected readonly IEnumerable<string> _supported;

        protected BaseAnalyzeDiagnosticReport()
        {
            _supported = GetSupportedAnalyzers();
        }

        public virtual IReadOnlyList<AnalyzerSummary> Analyze(ScannerResult report)
        {
            var filtered = report.Results
                .Where(x => _supported.Contains(x.Id))
                .ToList();
            var rollup = GetRollup(filtered, x => x.ParentComponentId);
            
            var summary = (from result in rollup
                    let healthy = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == ProbeResultStatus.Healthy)
                            .ConvertTo(),
                        CalcPercentage(result.Value, ProbeResultStatus.Healthy))
                    let unhealthy = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == ProbeResultStatus.Unhealthy)
                            .ConvertTo(),
                        CalcPercentage(result.Value, ProbeResultStatus.Unhealthy))
                    let warning = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == ProbeResultStatus.Warning)
                            .ConvertTo(),
                        CalcPercentage(result.Value, ProbeResultStatus.Warning))
                    let inconclusive = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == ProbeResultStatus.Inconclusive)
                            .ConvertTo(),
                        CalcPercentage(result.Value, ProbeResultStatus.Inconclusive))
                    select new AnalyzerSummaryImpl(result.Key, healthy, unhealthy, warning, inconclusive))
                .Cast<AnalyzerSummary>()
                .ToList();

            return summary;
        }

        protected abstract IEnumerable<string> GetSupportedAnalyzers();
        
        protected virtual decimal CalcPercentage(List<ProbeResultStatus> results, ProbeResultStatus status)
        {
            decimal totalCount = Convert.ToDecimal(results.Count);
            decimal statusCount = Convert.ToDecimal(results.Count(x => x == status));
            
            return Convert.ToDecimal(statusCount / totalCount * 100);
        }

        protected virtual IDictionary<string, List<ProbeResultStatus>> GetRollup(IReadOnlyList<ProbeResult> results,
            Func<ProbeResult, string> rollupKey)
        {
            var rollup = new Dictionary<string, List<ProbeResultStatus>>();

            for (int i = 0; i < results.Count; i++)
            {
                string key = rollupKey(results[i]);
                
                if (rollup.ContainsKey(key))
                    rollup[key].Add(results[i].Status);
                else
                    rollup.Add(key, new List<ProbeResultStatus> {results[i].Status});
            }

            return rollup;
        }


        class AnalyzerSummaryImpl :
            AnalyzerSummary
        {
            public AnalyzerSummaryImpl(string id, AnalyzerResult healthy, AnalyzerResult unhealthy,
                AnalyzerResult warning, AnalyzerResult inconclusive)
            {
                Id = id;
                Healthy = healthy;
                Unhealthy = unhealthy;
                Warning = warning;
                Inconclusive = inconclusive;
            }

            public string Id { get; }
            public AnalyzerResult Healthy { get; }
            public AnalyzerResult Unhealthy { get; }
            public AnalyzerResult Warning { get; }
            public AnalyzerResult Inconclusive { get; }
        }

        
        class AnalyzerResultImpl :
            AnalyzerResult
        {
            public AnalyzerResultImpl(uint total, decimal percentage)
            {
                Total = total;
                Percentage = percentage;
            }

            public uint Total { get; }
            public decimal Percentage { get; }
        }
    }
}
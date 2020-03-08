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
namespace HareDu.Analytics.Analyzers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Diagnostics;

    public abstract class BaseAnalyzeDiagnosticReport
    {
        protected readonly IEnumerable<string> _supportedAnalyzers;

        protected BaseAnalyzeDiagnosticReport()
        {
            _supportedAnalyzers = GetSupportedDiagnosticAnalyzers();
        }

        public virtual IReadOnlyList<AnalyzerSummary> Analyze(ScannerResult report)
        {
            var filtered = ApplyFilter(report.Results);
            var rollup = GetRollup(filtered, x => x.ParentComponentIdentifier);
            
            var summary = (from result in rollup
                    let green = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == DiagnosticProbeResultStatus.Healthy)
                            .ConvertTo(),
                        CalcPercentage(result.Value, DiagnosticProbeResultStatus.Healthy))
                    let red = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == DiagnosticProbeResultStatus.Unhealthy)
                            .ConvertTo(),
                        CalcPercentage(result.Value, DiagnosticProbeResultStatus.Unhealthy))
                    let yellow = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == DiagnosticProbeResultStatus.Warning)
                            .ConvertTo(),
                        CalcPercentage(result.Value, DiagnosticProbeResultStatus.Warning))
                    let inconclusive = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == DiagnosticProbeResultStatus.Inconclusive)
                            .ConvertTo(),
                        CalcPercentage(result.Value, DiagnosticProbeResultStatus.Inconclusive))
                    select new AnalyzerSummaryImpl(result.Key, green, red, yellow, inconclusive))
                .Cast<AnalyzerSummary>()
                .ToList();

            return summary;
        }

        protected abstract IEnumerable<string> GetSupportedDiagnosticAnalyzers();

        protected virtual IEnumerable<ProbeResult> ApplyFilter(IReadOnlyList<ProbeResult> results)
        {
            foreach (var result in results)
            {
                if (_supportedAnalyzers.Contains(result.ProbeIdentifier))
                    yield return result;
            }
        }
        
        protected virtual decimal CalcPercentage(List<DiagnosticProbeResultStatus> results, DiagnosticProbeResultStatus status)
        {
            decimal totalCount = Convert.ToDecimal(results.Count);
            decimal statusCount = Convert.ToDecimal(results.Count(x => x == status));
            
            return Convert.ToDecimal(statusCount / totalCount * 100);
        }

        protected virtual IDictionary<string, List<DiagnosticProbeResultStatus>> GetRollup(IEnumerable<ProbeResult> results,
            Func<ProbeResult, string> rollupKey)
        {
            var rollup = new Dictionary<string, List<DiagnosticProbeResultStatus>>();
            
            foreach (var result in results)
            {
                string key = rollupKey(result);
                
                if (rollup.ContainsKey(key))
                    rollup[key].Add(result.Status);
                else
                    rollup.Add(key, new List<DiagnosticProbeResultStatus> {result.Status});
            }

            return rollup;
        }


        class AnalyzerSummaryImpl :
            AnalyzerSummary
        {
            public AnalyzerSummaryImpl(string identifier, AnalyzerResult green, AnalyzerResult red, AnalyzerResult yellow, AnalyzerResult inconclusive)
            {
                Identifier = identifier;
                Green = green;
                Red = red;
                Yellow = yellow;
                Inconclusive = inconclusive;
            }

            public string Identifier { get; }
            public AnalyzerResult Green { get; }
            public AnalyzerResult Red { get; }
            public AnalyzerResult Yellow { get; }
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
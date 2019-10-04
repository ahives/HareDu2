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

        public virtual IReadOnlyList<AnalyzerSummary> Analyze(DiagnosticReport report)
        {
            var filtered = ApplyFilter(report.Results);
            var rollup = GetRollup(filtered, x => x.ParentComponentIdentifier);
            
            var summary = (from result in rollup
                    let green = new AnalyzerResultImpl(result.Value.Count(x => x == DiagnosticStatus.Green).ConvertTo(),
                        CalcPercentage(result.Value, DiagnosticStatus.Green))
                    let red = new AnalyzerResultImpl(result.Value.Count(x => x == DiagnosticStatus.Red).ConvertTo(),
                        CalcPercentage(result.Value, DiagnosticStatus.Red))
                    let yellow = new AnalyzerResultImpl(
                        result.Value.Count(x => x == DiagnosticStatus.Yellow).ConvertTo(),
                        CalcPercentage(result.Value, DiagnosticStatus.Yellow))
                    let inconclusive = new AnalyzerResultImpl(
                        result.Value.Count(x => x == DiagnosticStatus.Inconclusive).ConvertTo(),
                        CalcPercentage(result.Value, DiagnosticStatus.Inconclusive))
                    select new AnalyzerSummaryImpl(result.Key, green, red, yellow, inconclusive))
                .Cast<AnalyzerSummary>()
                .ToList();

            return summary;
        }

        public virtual bool IsSupported(IEnumerable<string> identifiers) => identifiers.Any(x => _supportedAnalyzers.Contains(x));
        
        public virtual bool IsSupported(string identifier) => _supportedAnalyzers.Any(x => x == identifier);

        protected abstract IEnumerable<string> GetSupportedDiagnosticAnalyzers();

        protected virtual IEnumerable<DiagnosticResult> ApplyFilter(IReadOnlyList<DiagnosticResult> results)
        {
            foreach (var result in results)
            {
                if (_supportedAnalyzers.Contains(result.AnalyzerIdentifier))
                    yield return result;
            }
        }
        
        protected virtual decimal CalcPercentage(List<DiagnosticStatus> results, DiagnosticStatus status)
        {
            decimal totalCount = Convert.ToDecimal(results.Count);
            decimal statusCount = Convert.ToDecimal(results.Count(x => x == status));
            
            return Convert.ToDecimal(statusCount / totalCount * 100);
        }

        protected virtual IDictionary<string, List<DiagnosticStatus>> GetRollup(IEnumerable<DiagnosticResult> results, Func<DiagnosticResult, string> rollupKey)
        {
            var rollup = new Dictionary<string, List<DiagnosticStatus>>();
            
            foreach (var result in results)
            {
                string key = rollupKey(result);
                
                if (rollup.ContainsKey(key))
                    rollup[key].Add(result.Status);
                else
                    rollup.Add(key, new List<DiagnosticStatus> {result.Status});
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
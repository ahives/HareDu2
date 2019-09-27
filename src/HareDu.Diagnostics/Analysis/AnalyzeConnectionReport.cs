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
    using Analyzers;

    public class AnalyzeConnectionReport :
        BaseAnalyzeDiagnosticReport,
        IAnalyzeDiagnosticReport
    {
        public IReadOnlyList<AnalyzerSummary> Analyze(DiagnosticReport report)
        {
            string identifier = typeof(ChannelThrottlingAnalyzer).GenerateIdentifier();
            var filtered = report.Results.Where(x => x.AnalyzerIdentifier == identifier);
            
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
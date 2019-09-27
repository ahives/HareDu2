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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class BaseAnalyzeDiagnosticReport
    {
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
    }
}
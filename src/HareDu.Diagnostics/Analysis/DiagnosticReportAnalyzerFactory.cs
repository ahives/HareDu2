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

    public class DiagnosticReportAnalyzerFactory :
        IDiagnosticReportAnalyzerFactory
    {
        readonly IReadOnlyList<IDiagnosticReportAnalyzer> _analyzers;

        public DiagnosticReportAnalyzerFactory(IReadOnlyList<IDiagnosticReportAnalyzer> analyzers)
        {
            _analyzers = analyzers;
        }

        public bool TryGet(string identifier, out IDiagnosticReportAnalyzer analyzer)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                analyzer = new DoNothingDiagnosticReportAnalyzer();
                return false;
            }

            analyzer = _analyzers.FirstOrDefault(x => x.IsSupported(identifier));
            if (analyzer != null)
                return true;
            
            analyzer = new DoNothingDiagnosticReportAnalyzer();
            return false;
        }
    }
}
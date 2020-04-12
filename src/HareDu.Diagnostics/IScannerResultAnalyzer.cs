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
    using System;
    using System.Collections.Generic;

    public interface IScannerResultAnalyzer
    {
        /// <summary>
        /// Given a <see cref="ScannerResult"/>, will aggregate and calculate the percentage per status of the result of diagnostic probes executing on a particular component.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="filterBy"></param>
        /// <returns></returns>
        IReadOnlyList<AnalyzerSummary> Analyze(ScannerResult report, Func<ProbeResult, string> filterBy);
        
        /// <summary>
        /// Registers an observer that receives the output in real-time as the analyzer executes.
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        IScannerResultAnalyzer RegisterObserver(IObserver<AnalyzerContext> observer);
    }
}
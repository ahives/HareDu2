namespace HareDu.Diagnostics.Extensions
{
    using System;
    using System.Collections.Generic;
    using Core.Extensions;

    public static class AnalyzerExtensions
    {
        /// <summary>
        /// Given a <see cref="ScannerResult"/>, will aggregate and calculate the percentage per status of the result of diagnostic probes executing on a particular component. If the analyzer is null, will return an empty summary.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="analyzer"></param>
        /// <param name="aggregationKey"></param>
        /// <returns></returns>
        public static IReadOnlyList<AnalyzerSummary> Analyze(this ScannerResult report, IScannerResultAnalyzer analyzer, Func<ProbeResult, string> aggregationKey)
            => analyzer.IsNotNull()
                ? analyzer.Analyze(report, aggregationKey)
                : DiagnosticCache.EmptyAnalyzerSummary;
    }
}
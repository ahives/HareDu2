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
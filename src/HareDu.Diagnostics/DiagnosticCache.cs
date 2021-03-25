namespace HareDu.Diagnostics
{
    using System.Collections.Generic;

    public static class DiagnosticCache
    {
        public static readonly IReadOnlyList<ProbeResult> EmptyProbeResults = new List<ProbeResult>();
        public static readonly ScannerResult EmptyScannerResult = new EmptyScannerResult();
        public static readonly IReadOnlyList<ProbeData> EmptyProbeData = new List<ProbeData>();
        public static readonly IReadOnlyList<AnalyzerSummary> EmptyAnalyzerSummary = new List<AnalyzerSummary>();
    }
}
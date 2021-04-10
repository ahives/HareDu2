namespace HareDu.Diagnostics.Scanners
{
    using System.Collections.Generic;
    using Probes;
    using Snapshotting;

    public interface DiagnosticScanner<in T>
        where T : Snapshot
    {
        /// <summary>
        /// Miscellaneous information pertinent to describing the diagnostic scanner.
        /// </summary>
        DiagnosticScannerMetadata Metadata { get; }

        /// <summary>
        /// Executes the diagnostic probes against the specified snapshot.
        /// </summary>
        /// <param name="snapshot"></param>
        /// <returns></returns>
        IReadOnlyList<ProbeResult> Scan(T snapshot);

        /// <summary>
        /// Configures the scanner.
        /// </summary>
        /// <param name="probes">Complete list of diagnostic probes.</param>
        void Configure(IReadOnlyList<DiagnosticProbe> probes);
    }
}
namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using Probes;
    using Scanners;
    using Snapshotting;

    public interface IScannerFactory
    {
        /// <summary>
        /// List of available diagnostic probes.
        /// </summary>
        IReadOnlyDictionary<string, DiagnosticProbe> Probes { get; }
        
        /// <summary>
        /// List of available diagnostic scanners.
        /// </summary>
        IReadOnlyDictionary<string, object> Scanners { get; }

        /// <summary>
        /// Try to return a diagnostic scanner.
        /// </summary>
        /// <param name="scanner"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool TryGet<T>(out DiagnosticScanner<T> scanner)
            where T : Snapshot;

        /// <summary>
        /// Connect a list of observers to the diagnostic scanner to receive notifications when a snapshot is taken.
        /// </summary>
        /// <param name="observers"></param>
        void RegisterObservers(IReadOnlyList<IObserver<ProbeContext>> observers);

        /// <summary>
        /// Connect an observer to the diagnostic scanner to receive notifications when a snapshot is taken.
        /// </summary>
        /// <param name="observer"></param>
        void RegisterObserver(IObserver<ProbeContext> observer);

        /// <summary>
        /// Try to register a diagnostic probe.
        /// </summary>
        /// <param name="probe"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool TryRegisterProbe<T>(T probe)
            where T : DiagnosticProbe;

        /// <summary>
        /// Try to register a diagnostic scanner.
        /// </summary>
        /// <param name="scanner"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool TryRegisterScanner<T>(DiagnosticScanner<T> scanner)
            where T : Snapshot;

        /// <summary>
        /// Try to register all diagnostic probes.
        /// </summary>
        /// <returns></returns>
        bool TryRegisterAllProbes();

        /// <summary>
        /// Try to register all diagnostic scanners.
        /// </summary>
        /// <returns></returns>
        bool TryRegisterAllScanners();
    }
}
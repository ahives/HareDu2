namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using Probes;
    using Scanners;
    using Snapshotting;

    public interface IScannerFactory
    {
        IReadOnlyDictionary<string, DiagnosticProbe> Probes { get; }
        
        IReadOnlyDictionary<string, object> Scanners { get; }
        
        bool TryGet<T>(out DiagnosticScanner<T> scanner)
            where T : Snapshot;

        void RegisterObservers(IReadOnlyList<IObserver<ProbeContext>> observers);

        void RegisterObserver(IObserver<ProbeContext> observer);

        bool RegisterProbe<T>(T probe)
            where T : DiagnosticProbe;

        bool RegisterScanner<T>(DiagnosticScanner<T> scanner)
            where T : Snapshot;

        bool TryRegisterAllProbes();

        bool TryRegisterAllScanners();
    }
}
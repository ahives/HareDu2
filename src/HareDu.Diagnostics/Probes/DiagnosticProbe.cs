namespace HareDu.Diagnostics.Probes
{
    using System;

    public interface DiagnosticProbe :
        IObservable<ProbeContext>
    {
        /// <summary>
        /// Miscellaneous information pertinent to describing the diagnostic probe.
        /// </summary>
        DiagnosticProbeMetadata Metadata { get; }
        
        ComponentType ComponentType { get; }
        
        ProbeCategory Category { get; }
        
        ProbeResult Execute<T>(T snapshot);
    }
}
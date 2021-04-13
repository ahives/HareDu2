namespace HareDu.Core.Configuration
{
    using System;

    public interface DiagnosticsConfigurator
    {
        void Probes(Action<DiagnosticProbesConfigurator> configurator);
    }
}
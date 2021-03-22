namespace HareDu.Model
{
    using System;

    public interface ConnectionClientProperties
    {
        string Assembly { get; }

        string AssemblyVersion { get; }

        ConnectionCapabilities Capabilities { get; }

        string ClientApi { get; }

        DateTimeOffset Connected { get; }

        string ConnectionName { get; }

        string Copyright { get; }

        string Host { get; }

        string Information { get; }

        string Platform { get; }

        string ProcessId { get; }

        string ProcessName { get; }

        string Product { get; }

        string Version { get; }
    }
}
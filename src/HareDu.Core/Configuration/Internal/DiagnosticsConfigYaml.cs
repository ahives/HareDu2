namespace HareDu.Core.Configuration.Internal
{
    using YamlDotNet.Serialization;

    class DiagnosticsConfigYaml
    {
        [YamlMember(Alias = "probes")]
        public DiagnosticProbeConfigYaml Probes { get; set; }
    }
}
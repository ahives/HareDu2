namespace HareDu.Core.Configuration.Internal
{
    using YamlDotNet.Serialization;

    class HareDuConfigYaml
    {
        [YamlMember(Alias = "broker")]
        public BrokerConfigYaml Broker { get; set; }

        [YamlMember(Alias = "diagnostics")]
        public DiagnosticsConfigYaml Diagnostics { get; set; }
    }
}
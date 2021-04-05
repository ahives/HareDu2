namespace HareDu.Internal.Model
{
    using System;
    using System.Text.Json.Serialization;

    class ConnectionClientPropertiesImpl
    {
        [JsonPropertyName("assembly")]
        public string Assembly { get; set; }

        [JsonPropertyName("assembly_version")]
        public string AssemblyVersion { get; set; }

        [JsonPropertyName("capabilities")]
        public ConnectionCapabilitiesImpl Capabilities { get; set; }

        [JsonPropertyName("client_api")]
        public string ClientApi { get; set; }

        [JsonPropertyName("connected")]
        public DateTimeOffset Connected { get; set; }

        [JsonPropertyName("connection_name")]
        public string ConnectionName { get; set; }

        [JsonPropertyName("copyright")]
        public string Copyright { get; set; }

        [JsonPropertyName("hostname")]
        public string Host { get; set; }

        [JsonPropertyName("information")]
        public string Information { get; set; }

        [JsonPropertyName("platform")]
        public string Platform { get; set; }

        [JsonPropertyName("process_id")]
        public string ProcessId { get; set; }

        [JsonPropertyName("process_name")]
        public string ProcessName { get; set; }

        [JsonPropertyName("product")]
        public string Product { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
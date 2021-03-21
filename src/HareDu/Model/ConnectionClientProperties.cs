namespace HareDu.Model
{
    using System;
    using System.Text.Json.Serialization;

    public interface ConnectionClientProperties
    {
        [JsonPropertyName("assembly")]
        string Assembly { get; }

        [JsonPropertyName("assembly_version")]
        string AssemblyVersion { get; }

        [JsonPropertyName("capabilities")]
        ConnectionCapabilities Capabilities { get; }

        [JsonPropertyName("client_api")]
        string ClientApi { get; }

        [JsonPropertyName("connected")]
        DateTimeOffset Connected { get; }

        [JsonPropertyName("connection_name")]
        string ConnectionName { get; }

        [JsonPropertyName("copyright")]
        string Copyright { get; }

        [JsonPropertyName("hostname")]
        string Host { get; }

        [JsonPropertyName("information")]
        string Information { get; }

        [JsonPropertyName("platform")]
        string Platform { get; }

        [JsonPropertyName("process_id")]
        string ProcessId { get; }

        [JsonPropertyName("process_name")]
        string ProcessName { get; }

        [JsonPropertyName("product")]
        string Product { get; }

        [JsonPropertyName("version")]
        string Version { get; }
    }
}
namespace HareDu.Internal.Model
{
    using System;
    using Core.Extensions;
    using HareDu.Model;

    class InternalConnectionClientProperties :
        ConnectionClientProperties
    {
        public InternalConnectionClientProperties(ConnectionClientPropertiesImpl obj)
        {
            Assembly = obj.Assembly;
            AssemblyVersion = obj.AssemblyVersion;
            Capabilities = obj.Capabilities.IsNotNull() ? new InternalCapabilities(obj.Capabilities) : default;
            ClientApi = obj.ClientApi;
            Connected = obj.Connected;
            ConnectionName = obj.ConnectionName;
            Copyright = obj.Copyright;
            Host = obj.Host;
            Information = obj.Information;
            Platform = obj.Platform;
            ProcessId = obj.ProcessId;
            ProcessName = obj.ProcessName;
            Product = obj.Product;
            Version = obj.Version;
        }

        public string Assembly { get; }
        public string AssemblyVersion { get; }
        public ConnectionCapabilities Capabilities { get; }
        public string ClientApi { get; }
        public DateTimeOffset Connected { get; }
        public string ConnectionName { get; }
        public string Copyright { get; }
        public string Host { get; }
        public string Information { get; }
        public string Platform { get; }
        public string ProcessId { get; }
        public string ProcessName { get; }
        public string Product { get; }
        public string Version { get; }
    }
}
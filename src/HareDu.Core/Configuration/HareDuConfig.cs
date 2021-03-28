namespace HareDu.Core.Configuration
{
    public interface HareDuConfig
    {
        BrokerConfig Broker { get; }
        
        DiagnosticsConfig Diagnostics { get; }
    }
}
namespace HareDu.Core.Configuration
{
    using System;

    public interface HareDuConfigurator
    {
        void Diagnostics(Action<DiagnosticsConfigurator> configurator);
        
        void Broker(Action<BrokerConfigurator> configurator);
    }
}
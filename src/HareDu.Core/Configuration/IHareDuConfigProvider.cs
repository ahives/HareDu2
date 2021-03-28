namespace HareDu.Core.Configuration
{
    using System;

    public interface IHareDuConfigProvider
    {
        HareDuConfig Configure(Action<HareDuConfigurator> configurator);
    }
}
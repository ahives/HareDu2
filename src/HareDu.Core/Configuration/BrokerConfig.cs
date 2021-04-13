namespace HareDu.Core.Configuration
{
    using System;

    public interface BrokerConfig
    {
        string Url { get; }
        
        TimeSpan Timeout { get; }
        
        BrokerCredentials Credentials { get; }
    }
}
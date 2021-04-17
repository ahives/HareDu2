namespace HareDu.Core.Configuration.Internal
{
    using System;

    public class InternalBrokerConfig
    {
        public string Url { get; set; }
        public TimeSpan Timeout { get; set; }
        public InternalBrokerCredentials Credentials { get; set; }
    }
}
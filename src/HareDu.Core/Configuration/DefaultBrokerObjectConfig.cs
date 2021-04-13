namespace HareDu.Core.Configuration
{
    using System;

    public class DefaultBrokerObjectConfig :
        BrokerConfig
    {
        public DefaultBrokerObjectConfig()
        {
            Url = "http://localhost:15672";
            Credentials = new BrokerCredentialsImpl("guest", "guest");
        }

        public string Url { get; }
        public TimeSpan Timeout { get; }
        public BrokerCredentials Credentials { get; }

        
        class BrokerCredentialsImpl :
            BrokerCredentials
        {
            public BrokerCredentialsImpl(string username, string password)
            {
                Username = username;
                Password = password;
            }

            public string Username { get; }
            public string Password { get; }
        }
    }
}
namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Common.Logging;
    using Internal.Serialization;
    using Model;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class HareDuClientTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            HareDuClientSettings config = new HareDuClientSettingsImpl(
                "http://localhost:15672",
                true,
                "HareDuLogger",
                new TimeSpan(0, 0, 0, 50), 
                new HareDuCredentialsImpl("guest", "guest"),
                true,
                3);
            
            using (StreamWriter sw = new StreamWriter("/users/albert/documents/git/haredu_config.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                SerializerCache.Serializer.Serialize(writer, config);
            }
        }

        [Test]
        public void Verify_can_init_client_from_json()
        {
            StreamReader reader = File.OpenText("/users/albert/documents/git/haredu_config.txt");

            string config = reader.ReadToEnd();

            IEnumerable<VirtualHostInfo> vhosts = HareDuFactory
                .Create(config)
                .Factory<VirtualHost>()
                .GetAll()
                .Where(x => x.Name == "HareDu");
            
            foreach (var vhost in vhosts)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public void Verify_can_init_client_from_behavior_description()
        {
            IEnumerable<VirtualHostInfo> vhosts = HareDuFactory
                .Create(x =>
                {
                    x.ConnectTo("http://localhost:15672");
                    x.Logging(s =>
                    {
                        s.Enable();
                        s.UseLogger("HareDuLogger");
                    });
                    x.UsingCredentials("guest", "guest");
                    x.TransientRetry(s =>
                    {
                        s.Enable();
                        s.RetryLimit(3);
                    });
                })
                .Factory<VirtualHost>()
                .GetAll()
                .Where(x => x.Name == "HareDu");
            
            foreach (var vhost in vhosts)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public void Verify_can_init_client_from_settings_object()
        {
            var vhosts = HareDuFactory
                .Create(() => new HareDuClientSettingsImpl(
                    "http://localhost:15672",
                    true,
                    "HareDuLogger",
                    new HareDuCredentialsImpl("guest", "guest"),
                    true,
                    3))
                .Factory<VirtualHost>()
                .GetAll()
                .Where(x => x.Name == "HareDu");
                        
            foreach (var vhost in vhosts)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        
        class HareDuCredentialsImpl :
            HareDuCredentials
        {
            public HareDuCredentialsImpl(string username, string password)
            {
                Username = username;
                Password = password;
            }

            public string Username { get; }
            public string Password { get; }
        }

        
        class HareDuClientSettingsImpl :
            HareDuClientSettings
        {
            public HareDuClientSettingsImpl(string host, bool enableLogger, string loggerName, TimeSpan timeout, HareDuCredentials credentials, bool enableTransientRetry, int retryLimit)
            {
                Host = host;
                EnableLogger = enableLogger;
                LoggerName = loggerName;
                Timeout = timeout;
                Credentials = credentials;
                EnableTransientRetry = enableTransientRetry;
                RetryLimit = retryLimit;
            }

            public HareDuClientSettingsImpl(string host, bool enableLogger, string loggerName, HareDuCredentials credentials, bool enableTransientRetry, int retryLimit)
            {
                Host = host;
                EnableLogger = enableLogger;
                LoggerName = loggerName;
                Credentials = credentials;
                EnableTransientRetry = enableTransientRetry;
                RetryLimit = retryLimit;
            }

            public string Host { get; }
            public bool EnableLogger { get; }
            public string LoggerName { get; }
            public ILog Logger { get; }
            public TimeSpan Timeout { get; }
            public HareDuCredentials Credentials { get; }
            public bool EnableTransientRetry { get; }
            public int RetryLimit { get; }
        }
    }
}
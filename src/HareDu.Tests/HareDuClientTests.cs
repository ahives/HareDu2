namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Common.Logging;
    using Configuration;
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
//            string config;
//
//            using (StreamReader reader = File.OpenText("/users/albert/documents/git/haredu_config.txt"))
//            {
//                config = reader.ReadToEnd();
//            }

            string config = @"{
	'rmqServerUrl':'http://localhost:15672',
            'timeout':'00:00:50',
            'logger':{
                'enable':true,
                'name':'HareDuLogger'
            },
            'credentials':{
                'username':'guest',
                'password':'guest'
            },
            'transientRetry':{
                'enable':true,
                'limit':3
            }
        }";

            IEnumerable<VirtualHostInfo> vhosts = HareDuClient
                .Initialize(config)
                .Factory<VirtualHost>()
                .GetAllAsync()
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
            IEnumerable<VirtualHostInfo> vhosts = HareDuClient
                .Initialize(x =>
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
                        s.Limit(3);
                    });
                })
                .Factory<VirtualHost>()
                .GetAllAsync()
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
            var vhosts = HareDuClient
                .Initialize(() => new HareDuClientSettingsImpl(
                    "http://localhost:15672",
                    true,
                    "HareDuLogger",
                    new HareDuCredentialsImpl("guest", "guest"),
                    true,
                    3))
                .Factory<VirtualHost>()
                .GetAllAsync()
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
            public HareDuClientSettingsImpl(string rmqServerUrl, bool enableLogger, string loggerName,
                TimeSpan timeout, HareDuCredentials credentials, bool enableTransientRetry, int retryLimit)
            {
                RabbitMqServerUrl = rmqServerUrl;
                Timeout = timeout;
                Credentials = credentials;
                LoggerSettings = new HareDuLoggerSettingsImpl(enableLogger, loggerName);
                TransientRetrySettings = new HareDuTransientRetrySettingsImpl(enableTransientRetry, retryLimit);
            }
            
            public HareDuClientSettingsImpl(string rmqServerUrl, bool enableLogger, string loggerName,
                HareDuCredentials credentials, bool enableTransientRetry, int retryLimit)
            {
                RabbitMqServerUrl = rmqServerUrl;
                Credentials = credentials;
                LoggerSettings = new HareDuLoggerSettingsImpl(enableLogger, loggerName);
                TransientRetrySettings = new HareDuTransientRetrySettingsImpl(enableTransientRetry, retryLimit);
            }

            public string RabbitMqServerUrl { get; }
            public TimeSpan Timeout { get; }
            public HareDuLoggerSettings LoggerSettings { get; }
            public HareDuCredentials Credentials { get; }
            public HareDuTransientRetrySettings TransientRetrySettings { get; }


            class HareDuTransientRetrySettingsImpl :
                HareDuTransientRetrySettings
            {
                public HareDuTransientRetrySettingsImpl(bool enable, int retryLimit)
                {
                    Enable = enable;
                    RetryLimit = retryLimit;
                }

                public bool Enable { get; }
                public int RetryLimit { get; }
            }


            class HareDuLoggerSettingsImpl :
                HareDuLoggerSettings
            {
                public HareDuLoggerSettingsImpl(bool enableLogger, string loggerName)
                {
                    Enable = enableLogger;
                    Name = loggerName;
                }

                public bool Enable { get; }
                public string Name { get; }
                public ILog Logger { get; }
            }
        }
    }
}
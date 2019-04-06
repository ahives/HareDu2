﻿namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Configuration;
    using Extensions;
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
                new TimeSpan(0, 0, 0, 50),
                new HareDuCredentialsImpl("guest", "guest"));
            
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
                .Resource<VirtualHost>()
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
            IEnumerable<VirtualHostInfo> vhosts = HareDuClient
                .Initialize(x =>
                {
                    x.ConnectTo("http://localhost:15672");
                    x.UsingCredentials("guest", "guest");
                })
                .Resource<VirtualHost>()
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
            var vhosts = HareDuClient
                .Initialize(() => new HareDuClientSettingsImpl(
                    "http://localhost:15672",
                    new HareDuCredentialsImpl("guest", "guest")))
                .Resource<VirtualHost>()
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
            public HareDuClientSettingsImpl(string rmqServerUrl, TimeSpan timeout, HareDuCredentials credentials)
            {
                RabbitMqServerUrl = rmqServerUrl;
                Timeout = timeout;
                Credentials = credentials;
            }
            
            public HareDuClientSettingsImpl(string rmqServerUrl, HareDuCredentials credentials)
            {
                RabbitMqServerUrl = rmqServerUrl;
                Credentials = credentials;
            }

            public string RabbitMqServerUrl { get; }
            public TimeSpan Timeout { get; }
            public HareDuCredentials Credentials { get; }
        }
    }
}
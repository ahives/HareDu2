// Copyright 2013-2018 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using Configuration;
    using Exceptions;
    using Extensions;
    using Internal;
    using Internal.Serialization;
    using Newtonsoft.Json;

    public static class HareDuClient
    {
        public static HareDuFactory Initialize(Action<HareDuClientConfigurationProvider> configuration)
        {
            if (configuration == null)
                throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");
            
            try
            {
                var init = new HareDuClientConfigurationProviderImpl();
                configuration(init);

                HareDuClientSettings settings = init.Settings.Value;

                ValidateSettings(settings);

                HttpClient client = GetHttpClient(settings);
                
                HareDuFactory factory = new HareDuFactoryImpl(client, settings);

                return factory;
            }
            catch (Exception e)
            {
                throw new HareDuClientInitException("Unable to initialize the HareDu client.", e);
            }
        }

        public static HareDuFactory Initialize(string configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration))
                throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

            try
            {
                HareDuClientSettings settings = SerializerCache.Deserializer.Deserialize<HareDuClientSettings>(new JsonTextReader(new StringReader(configuration)));

                ValidateSettings(settings);

                HttpClient client = GetHttpClient(settings);
                
                HareDuFactory factory = new HareDuFactoryImpl(client, settings);

                return factory;
            }
            catch (Exception e)
            {
                throw new HareDuClientInitException("Unable to initialize the HareDu client.", e);
            }
        }

        public static HareDuFactory Initialize(Func<HareDuClientSettings> configuration)
        {
            if (configuration == null)
                throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");
            
            try
            {
                HareDuClientSettings settings = configuration();

                ValidateSettings(settings);

                HttpClient client = GetHttpClient(settings);
                
                HareDuFactory factory = new HareDuFactoryImpl(client, settings);

                return factory;
            }
            catch (Exception e)
            {
                throw new HareDuClientInitException("Unable to initialize the HareDu client.", e);
            }
        }

        static void ValidateSettings(HareDuClientSettings settings)
        {
            if (settings.IsNull())
                throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

            if (settings.Credentials.IsNull() || string.IsNullOrWhiteSpace(settings.Credentials.Username) || string.IsNullOrWhiteSpace(settings.Credentials.Password))
                throw new UserCredentialsMissingException("Username and password are required and cannot be empty.");
            
            if (string.IsNullOrWhiteSpace(settings.RabbitMqServerUrl))
                throw new HostUrlMissingException("Host URL is required and cannot be empty.");
        }

        static HttpClient GetHttpClient(HareDuClientSettings settings)
        {
            var uri = new Uri($"{settings.RabbitMqServerUrl}/");
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(settings.Credentials.Username, settings.Credentials.Password)
            };
            
            var client = new HttpClient(handler){BaseAddress = uri};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (settings.Timeout != TimeSpan.Zero)
                client.Timeout = settings.Timeout;

            return client;
        }


        class HareDuClientConfigurationProviderImpl :
            HareDuClientConfigurationProvider
        {
            static string _rmqServerUrl;
            static TimeSpan _timeout;
            static string _username;
            static string _password;

            public Lazy<HareDuClientSettings> Settings { get; }

            public HareDuClientConfigurationProviderImpl()
            {
                Settings = new Lazy<HareDuClientSettings>(
                        () => new HareDuClientSettingsImpl(_rmqServerUrl, _timeout, _username, _password), LazyThreadSafetyMode.PublicationOnly);
            }

            public void ConnectTo(string rmqServerUrl) => _rmqServerUrl = rmqServerUrl;

            public void TimeoutAfter(TimeSpan timeout) => _timeout = timeout;

            public void UsingCredentials(string username, string password)
            {
                _username = username;
                _password = password;
            }


            class HareDuClientSettingsImpl :
                HareDuClientSettings
            {
                public HareDuClientSettingsImpl(string rmqServerUrl, TimeSpan timeout, string username, string password)
                {
                    RabbitMqServerUrl = rmqServerUrl;
                    Timeout = timeout;
                    Credentials = new HareDuCredentialsImpl(username, password);
                }

                public string RabbitMqServerUrl { get; }
                public TimeSpan Timeout { get; }
                public HareDuCredentials Credentials { get; }



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
            }
        }
    }
}
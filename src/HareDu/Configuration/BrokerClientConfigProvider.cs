// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Configuration
{
    using System;
    using System.Threading;
    using Core.Configuration;
    using Core.Extensions;

    public class BrokerClientConfigProvider :
        IBrokerClientConfigProvider
    {
        public HareDuClientSettings Init(Action<ClientConfigProvider> configuration)
        {
            if (configuration == null)
                throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

            var init = new ClientConfigProviderImpl();
            configuration(init);

            HareDuClientSettings settings = init.Settings.Value;

            if (settings.IsNull() ||
                settings.Credentials.IsNull() ||
                string.IsNullOrWhiteSpace(settings.Credentials.Username) ||
                string.IsNullOrWhiteSpace(settings.Credentials.Password) ||
                string.IsNullOrWhiteSpace(settings.BrokerUrl))
            {
                return BrokerObjectConfigCache.Default;
            }

            return settings;
        }

        public bool TryGet(out HareDuClientSettings settings)
        {
            settings = BrokerObjectConfigCache.Default;
            return true;
        }


        class ClientConfigProviderImpl :
            ClientConfigProvider
        {
            string _rmqServerUrl;
            TimeSpan _timeout;
            string _username;
            string _password;

            public Lazy<HareDuClientSettings> Settings { get; }

            public ClientConfigProviderImpl()
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
                    BrokerUrl = rmqServerUrl;
                    Timeout = timeout;
                    Credentials = new HareDuCredentialsImpl(username, password);
                }

                public string BrokerUrl { get; }
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
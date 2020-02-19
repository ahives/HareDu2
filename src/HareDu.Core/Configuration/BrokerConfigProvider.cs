// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.Core.Configuration
{
    using System;
    using System.IO;
    using System.Threading;
    using Extensions;

    public class BrokerConfigProvider :
        IBrokerConfigProvider
    {
        public BrokerConfig Configure(Action<BrokerConfigurator> configurator)
        {
            if (configurator == null)
                return ConfigCache.Default.Broker;
            
            var impl = new BrokerConfiguratorImpl();
            configurator(impl);

            BrokerConfig config = impl.Settings.Value;

            return Validate(config) ? config : ConfigCache.Default.Broker;
        }

        bool Validate(BrokerConfig config)
            => !config.Credentials.IsNull() &&
               !string.IsNullOrWhiteSpace(config.Credentials.Username) &&
               !string.IsNullOrWhiteSpace(config.Credentials.Password) &&
               !string.IsNullOrWhiteSpace(config.Url);


        class BrokerConfiguratorImpl :
            BrokerConfigurator
        {
            string _brokerUrl;
            TimeSpan _timeout;
            string _username;
            string _password;

            public Lazy<BrokerConfig> Settings { get; }

            public BrokerConfiguratorImpl()
            {
                Settings = new Lazy<BrokerConfig>(
                    () => new BrokerConfigImpl(_brokerUrl, _timeout, _username, _password), LazyThreadSafetyMode.PublicationOnly);
            }

            public void ConnectTo(string url) => _brokerUrl = url;

            public void TimeoutAfter(TimeSpan timeout) => _timeout = timeout;

            public void UsingCredentials(string username, string password)
            {
                _username = username;
                _password = password;
            }


            class BrokerConfigImpl :
                BrokerConfig
            {
                public BrokerConfigImpl(string brokerUrl, TimeSpan timeout, string username, string password)
                {
                    Url = brokerUrl;
                    Timeout = timeout;

                    if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                        Credentials = new BrokerCredentialsImpl(username, password);
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
    }
}
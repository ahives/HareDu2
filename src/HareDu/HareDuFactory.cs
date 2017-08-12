// Copyright 2013-2017 Albert L. Hives
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
    using System.Threading;
    using Common.Logging;
    using Exceptions;
    using Internal;
    using Internal.Serialization;
    using Newtonsoft.Json;

    public static class HareDuFactory
    {
        public static HareDuClient Create(Action<HareDuClientBehavior> behavior)
        {
            try
            {
                var init = new HareDuClientBehaviorImpl();
                behavior(init);

                HareDuClientSettings settings = init.Settings.Value;

                if (init.Settings == null || settings == null)
                    throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                var client = new HareDuClientImpl(settings);

                return client;
            }
            catch (Exception e)
            {
                throw new HareDuClientInitException("Unable to initialize the HareDu client.", e);
            }
        }

        public static HareDuClient Create(string settings)
        {
            try
            {
                var clientSettings = SerializerCache.Deserializer.Deserialize<HareDuClientSettings>(new JsonTextReader(new StringReader(settings)));

                if (clientSettings == null)
                    throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                var client = new HareDuClientImpl(clientSettings);

                return client;
            }
            catch (Exception e)
            {
                throw new HareDuClientInitException("Unable to initialize the HareDu client.", e);
            }
        }

        public static HareDuClient Create(Func<HareDuClientSettings> settings)
        {
            try
            {
                if (settings == null)
                    throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");
                
                HareDuClientSettings clientSettings = settings();

                var client = new HareDuClientImpl(clientSettings);

                return client;
            }
            catch (Exception e)
            {
                throw new HareDuClientInitException("Unable to initialize the HareDu client.", e);
            }
        }


        class HareDuClientBehaviorImpl :
            HareDuClientBehavior
        {
            static string _host;
            static ILog _logger;
            static TimeSpan _timeout;
            static int _retryLimit;
            static bool _enableTransientRetry;
            static bool _enableLogger;
            static string _loggerName;
            static string _username;
            static string _password;

            public Lazy<HareDuClientSettings> Settings { get; }

            public HareDuClientBehaviorImpl() => Settings = new Lazy<HareDuClientSettings>(Init, LazyThreadSafetyMode.PublicationOnly);

            static HareDuClientSettings Init()
                => new HareDuClientSettingsImpl(_host, _enableLogger, _logger, _loggerName, _timeout, _username, _password, _enableTransientRetry, _retryLimit);

            public void ConnectTo(string host)
            {
                if (string.IsNullOrWhiteSpace(host))
                    throw new HostUrlMissingException("Host URL is required and cannot be empty.");
                
                _host = host;
            }

            public void Logging(Action<LoggerSettings> settings)
            {
                var impl = new LoggerSettingsImpl();
                settings(impl);

                _enableLogger = impl.IsEnabled;
                _logger = impl.Logger;
                _loggerName = impl.LoggerName;
            }

            public void TimeoutAfter(TimeSpan timeout) => _timeout = timeout;

            public void UsingCredentials(string username, string password)
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    throw new UserCredentialsMissingException("Username and password are required and cannot be empty.");
                
                _username = username;
                _password = password;
            }

            public void TransientRetry(Action<TransientRetrySettings> settings)
            {
                var impl = new TransientRetrySettingsImpl();
                settings(impl);

                _retryLimit = impl.Limit;
                _enableTransientRetry = impl.EnableTransientRetry;
            }

            
            class TransientRetrySettingsImpl :
                TransientRetrySettings
            {
                public int Limit { get; private set; }
                public bool EnableTransientRetry { get; private set; }

                public void Enable() => EnableTransientRetry = true;

                public void RetryLimit(int retryLimit) => Limit = retryLimit;
            }


            class HareDuClientSettingsImpl :
                HareDuClientSettings
            {
                public HareDuClientSettingsImpl(string host, bool enableLogger, ILog logger, string loggerName,
                    TimeSpan timeout, string username, string password, bool enableTransientRetry, int retryLimit)
                {
                    Host = host;
                    EnableLogger = enableLogger;
                    Logger = logger;
                    Timeout = timeout;
                    Credentials = new HareDuCredentialsImpl(username, password);
                    RetryLimit = retryLimit;
                    EnableTransientRetry = enableTransientRetry;
                    LoggerName = loggerName;
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


            class LoggerSettingsImpl :
                LoggerSettings
            {
                public string LoggerName { get; private set; }
                public bool IsEnabled { get; private set; }
                public ILog Logger { get; private set; }

                public void Enable() => IsEnabled = true;

                public void UseLogger(string name) => LoggerName = name;
                
                public void UseLogger(ILog logger) => Logger = logger;
            }
        }
    }
}
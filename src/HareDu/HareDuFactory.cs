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
    using System.Threading;
    using Common.Logging;
    using Exceptions;
    using Internal;

    public static class HareDuFactory
    {
        public static HareDuClient Create(Action<HareDuClientBehavior> behavior)
        {
            try
            {
                var init = new HareDuClientBehaviorImpl();
                behavior(init);

                var settings = init.Settings.Value;

                if (string.IsNullOrWhiteSpace(settings.Host))
                    throw new HostUrlMissingException("Host URL was missing.");
                
                if (string.IsNullOrWhiteSpace(settings.Credentials.Username) || string.IsNullOrWhiteSpace(settings.Credentials.Password))
                    throw new UserCredentialsMissingException("Username and/or password was missing.");

                var client = new HareDuClientImpl(settings);

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
            static HareDuCredentials _credentials;
            static int _retryLimit;
            static bool _enableTransientRetry;
            
            public Lazy<HareDuClientSettings> Settings { get; }

            public HareDuClientBehaviorImpl() => Settings = new Lazy<HareDuClientSettings>(InitClientSettings, LazyThreadSafetyMode.PublicationOnly);

            static HareDuClientSettings InitClientSettings() => new HareDuClientSettingsImpl(_host, _logger, _timeout, _credentials, _enableTransientRetry, _retryLimit);

            public void ConnectTo(string host) => _host = host;

            public void EnableLogging(Action<LoggerSettings> logger)
            {
                if (_logger != null)
                    return;
                
                var impl = new LoggerSettingsImpl();
                logger(impl);
                    
                _logger = LogManager.GetLogger(impl.Target);
            }

            public void TimeoutAfter(TimeSpan timeout) => _timeout = timeout;

            public void UsingCredentials(string username, string password) =>
                _credentials = new HareDuCredentialsImpl(username, password);

            public void TransientRetry(Action<TransientRetrySettings> settings)
            {
                if (_retryLimit > 0)
                    return;

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

                public void Enable(bool enableTransientRetry) => EnableTransientRetry = enableTransientRetry;

                public void RetryLimit(int retryLimit) => Limit = retryLimit;
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
                public HareDuClientSettingsImpl(string host, ILog logger, TimeSpan timeout, HareDuCredentials credentials, bool enableTransientRetry, int retryLimit)
                {
                    Host = host;
                    Logger = logger;
                    Timeout = timeout;
                    Credentials = credentials;
                    RetryLimit = retryLimit;
                    EnableTransientRetry = enableTransientRetry;
                }

                public string Host { get; }
                public ILog Logger { get; }
                public TimeSpan Timeout { get; }
                public HareDuCredentials Credentials { get; }
                public bool EnableTransientRetry { get; }
                public int RetryLimit { get; }
            }


            class LoggerSettingsImpl :
                LoggerSettings
            {
                public string Target { get; private set; }

                public void Logger(string name) => Target = name;
            }
        }
    }
}
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
namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Core;
    using Core.Configuration;
    using Core.Exceptions;
    using Diagnostics;
    using Internal;

    public static class SnapshotClient
    {
        public static ISnapshotFactory Init(Action<SnapshotClientConfigProvider> configuration)
        {
            if (configuration == null)
                throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

            try
            {
                var init = new SnapshotClientConfigProviderImpl();
                configuration(init);

                var resourceFactory = ResourceClient.Init(() => init.Settings.Value);
                
                SnapshotFactory.Instance.Init(resourceFactory, init.Observers.Value);

                return SnapshotFactory.Instance;
            }
            catch (Exception e)
            {
                throw new HareDuClientInitException("Unable to initialize the HareDu client.", e);
            }
        }

        class SnapshotClientConfigProviderImpl :
            SnapshotClientConfigProvider
        {
            string _rmqServerUrl;
            TimeSpan _timeout;
            string _username;
            string _password;
            readonly List<IObserver<DiagnosticContext>> _observers;

            public Lazy<HareDuClientSettings> Settings { get; }
            public Lazy<List<IObserver<DiagnosticContext>>> Observers { get; }

            public SnapshotClientConfigProviderImpl()
            {
                _observers = new List<IObserver<DiagnosticContext>>();
                
                Settings = new Lazy<HareDuClientSettings>(
                        () => new HareDuClientSettingsImpl(_rmqServerUrl, _timeout, _username, _password), LazyThreadSafetyMode.PublicationOnly);
                Observers = new Lazy<List<IObserver<DiagnosticContext>>>(() => _observers, LazyThreadSafetyMode.PublicationOnly);
            }

            public void ConnectTo(string rmqServerUrl) => _rmqServerUrl = rmqServerUrl;

            public void TimeoutAfter(TimeSpan timeout) => _timeout = timeout;

            public void UsingCredentials(string username, string password)
            {
                _username = username;
                _password = password;
            }

            public void RegisterObservers(IList<IObserver<DiagnosticContext>> observers)
            {
                for (int i = 0; i < observers.Count; i++)
                {
                    if (!_observers.Contains(observers[i]))
                        _observers.Add(observers[i]);
                }
            }

            public void RegisterObserver(IObserver<DiagnosticContext> observer)
            {
                if (!_observers.Contains(observer))
                    _observers.Add(observer);
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
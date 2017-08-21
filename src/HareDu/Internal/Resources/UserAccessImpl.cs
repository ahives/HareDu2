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
namespace HareDu.Internal.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Exceptions;
    using Model;

    internal class UserAccessImpl :
        ResourceBase,
        UserAccess
    {
        public UserAccessImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<UserAccessInfo>>> GetAll(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/users";

            LogInfo($"Sent request to return all user information on current RabbitMQ server");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<UserAccessInfo>> result = await response.GetResponse<IEnumerable<UserAccessInfo>>();

            return result;
        }

        public async Task<Result> Create(Action<UserAccessDefinition> definition, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new UserAccessDefinitionImpl();
            definition(impl);

            UserAccessSettings settings = impl.Settings.Value;

            if (string.IsNullOrWhiteSpace(impl.Username))
                throw new UserCredentialsMissingException("The username and/or password is missing.");

            if (string.IsNullOrWhiteSpace(impl.VirtualHost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            string sanitizedVHost = impl.VirtualHost.SanitizeVirtualHostName();

            string url = $"api/permissions/{sanitizedVHost}/{impl.Username}";

            LogInfo($"Sent request to RabbitMQ server to create user '{impl.Username}'");

            HttpResponseMessage response = await HttpPut(url, settings, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string vhost, string username, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(username))
                throw new UserCredentialsMissingException("The username and/or password is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/permissions/{sanitizedVHost}/{username}";

            LogInfo($"Sent request to RabbitMQ server to create user '{username}'");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        
        class UserAccessDefinitionImpl :
            UserAccessDefinition
        {
            static string _configurePattern;
            static string _writePattern;
            static string _readPattern;
            
            public Lazy<UserAccessSettings> Settings { get; }
            public string VirtualHost { get; private set; }
            public string Username { get; private set; }

            public UserAccessDefinitionImpl()
                => Settings = new Lazy<UserAccessSettings>(Init, LazyThreadSafetyMode.PublicationOnly);

            UserAccessSettings Init() => new UserAccessSettingsImpl(_configurePattern, _writePattern, _readPattern);

            public void Configure(Action<UserAccessConfiguration> definition)
            {
                var impl = new UserAccessConfigurationImpl();
                definition(impl);

                _configurePattern = impl.ConfigurePattern;
                _writePattern = impl.WritePattern;
                _readPattern = impl.ReadPattern;
            }

            public void OnUser(string username) => Username = username;

            public void OnVirtualHost(string vhost) => VirtualHost = vhost;


            class UserAccessSettingsImpl :
                UserAccessSettings
            {
                public UserAccessSettingsImpl(string configurePattern, string writePattern, string readPattern)
                {
                    Configure = configurePattern;
                    Write = writePattern;
                    Read = readPattern;
                }

                public string Configure { get; }
                public string Write { get; }
                public string Read { get; }
            }

            
            class UserAccessConfigurationImpl :
                UserAccessConfiguration
            {
                public string ConfigurePattern { get; private set; }
                public string ReadPattern { get; private set; }
                public string WritePattern { get; private set; }
                
                public void UsingConfigurePattern(string pattern) => ConfigurePattern = pattern;

                public void UsingWritePattern(string pattern) => WritePattern = pattern;

                public void UsingReadPattern(string pattern) => ReadPattern = pattern;
            }
        }
    }
}
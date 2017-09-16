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

    internal class UserPermissionsImpl :
        ResourceBase,
        UserPermissions
    {
        public UserPermissionsImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<UserAccessInfo>>> GetAllAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/users";

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<UserAccessInfo>> result = await response.GetResponse<IEnumerable<UserAccessInfo>>();

            LogInfo($"Sent request to return all user information on current RabbitMQ server");

            return result;
        }

        public async Task<Result> CreateAsync(Action<UserPermissionsCreateAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new UserPermissionsCreateActionImpl();
            action(impl);

            DefinedUserPermissions settings = impl.Settings.Value;

            string username = impl.Username.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(username))
                throw new UserCredentialsMissingException("The username and/or password is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/permissions/{sanitizedVHost}/{username}";

            HttpResponseMessage response = await HttpPut(url, settings, cancellationToken);
            Result result = await response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to create user '{username}'");

            return result;
        }

        public async Task<Result> DeleteAsync(Action<UserPermissionsDeleteAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new UserPermissionsDeleteActionImpl();
            action(impl);

            string username = impl.Username.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(username))
                throw new UserCredentialsMissingException("The username and/or password is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/permissions/{sanitizedVHost}/{username}";

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = await response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to create user '{username}'");

            return result;
        }

        
        class UserPermissionsDeleteActionImpl :
            UserPermissionsDeleteAction
        {
            static string _vhost;
            static string _user;
            
            public Lazy<string> Username { get; }
            public Lazy<string> VirtualHost { get; }

            public UserPermissionsDeleteActionImpl()
            {
                Username = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Targeting(Action<UserPermissionsTarget> target)
            {
                var impl = new UserPermissionsTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            }

            public void User(string name) => _user = name;

            
            class UserPermissionsTargetImpl :
                UserPermissionsTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string vhost) => VirtualHostName = vhost;
            }
        }

        
        class UserPermissionsCreateActionImpl :
            UserPermissionsCreateAction
        {
            static string _configurePattern;
            static string _writePattern;
            static string _readPattern;
            static string _vhost;
            static string _user;

            public Lazy<DefinedUserPermissions> Settings { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<string> Username { get; }

            public UserPermissionsCreateActionImpl()
            {
                Settings = new Lazy<DefinedUserPermissions>(
                    () => new DefinedUserPermissionsImpl(_configurePattern, _writePattern, _readPattern), LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                Username = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
            }

            public void User(string username) => _user = username;

            public void Configure(Action<UserPermissionsConfiguration> definition)
            {
                var impl = new UserPermissionsConfigurationImpl();
                definition(impl);

                _configurePattern = impl.ConfigurePattern;
                _writePattern = impl.WritePattern;
                _readPattern = impl.ReadPattern;
            }

            public void Targeting(Action<UserPermissionsTarget> target)
            {
                var impl = new UserPermissionsTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            }

            
            class UserPermissionsTargetImpl :
                UserPermissionsTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string vhost) => VirtualHostName = vhost;
            }


            class DefinedUserPermissionsImpl :
                DefinedUserPermissions
            {
                public DefinedUserPermissionsImpl(string configurePattern, string writePattern, string readPattern)
                {
                    Configure = configurePattern;
                    Write = writePattern;
                    Read = readPattern;
                }

                public string Configure { get; }
                public string Write { get; }
                public string Read { get; }
            }

            
            class UserPermissionsConfigurationImpl :
                UserPermissionsConfiguration
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
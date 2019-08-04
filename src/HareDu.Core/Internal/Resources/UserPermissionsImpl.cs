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
namespace HareDu.Core.Internal.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensions;
    using Model;

    class UserPermissionsImpl :
        ResourceBase,
        UserPermissions
    {
        public UserPermissionsImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<UserPermissionsInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/users";
            
            ResultList<UserPermissionsInfo> result = await GetAll<UserPermissionsInfo>(url, cancellationToken);

            return result;
        }

        public async Task<Result> Create(Action<UserPermissionsCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserPermissionsCreateActionImpl();
            action(impl);

            UserPermissionsDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/permissions/{SanitizeVirtualHostName(impl.VirtualHost.Value)}/{impl.Username.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString()));

            Result result = await Put(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result> Delete(Action<UserPermissionsDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new UserPermissionsDeleteActionImpl();
            action(impl);

            string url = $"api/permissions/{SanitizeVirtualHostName(impl.VirtualHost.Value)}/{impl.Username.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, null));

            Result result = await Delete(url, cancellationToken);

            return result;
        }

        
        class UserPermissionsDeleteActionImpl :
            UserPermissionsDeleteAction
        {
            string _vhost;
            string _user;
            readonly List<Error> _errors;

            public Lazy<string> Username { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<List<Error>> Errors { get; }

            public UserPermissionsDeleteActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Username = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Targeting(Action<UserPermissionsTarget> target)
            {
                var impl = new UserPermissionsTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;

                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            public void User(string name)
            {
                _user = name;
            
                if (string.IsNullOrWhiteSpace(_user))
                    _errors.Add(new ErrorImpl("The username and/or password is missing."));
            }

            
            class UserPermissionsTargetImpl :
                UserPermissionsTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }
        }

        
        class UserPermissionsCreateActionImpl :
            UserPermissionsCreateAction
        {
            string _configurePattern;
            string _writePattern;
            string _readPattern;
            string _vhost;
            string _user;
            readonly List<Error> _errors;

            public Lazy<UserPermissionsDefinition> Definition { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<string> Username { get; }
            public Lazy<List<Error>> Errors { get; }

            public UserPermissionsCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<UserPermissionsDefinition>(
                    () => new UserPermissionsDefinitionImpl(_configurePattern, _writePattern, _readPattern), LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                Username = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
            }

            public void User(string username)
            {
                _user = username;
            
                if (string.IsNullOrWhiteSpace(_user))
                    _errors.Add(new ErrorImpl("The username and/or password is missing."));
            }

            public void Configure(Action<UserPermissionsConfiguration> configure)
            {
                var impl = new UserPermissionsConfigurationImpl();
                configure(impl);

                _configurePattern = impl.ConfigurePattern;
                _writePattern = impl.WritePattern;
                _readPattern = impl.ReadPattern;
            }

            public void Targeting(Action<UserPermissionsTarget> target)
            {
                var impl = new UserPermissionsTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;

                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            
            class UserPermissionsTargetImpl :
                UserPermissionsTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }


            class UserPermissionsDefinitionImpl :
                UserPermissionsDefinition
            {
                public UserPermissionsDefinitionImpl(string configurePattern, string writePattern, string readPattern)
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
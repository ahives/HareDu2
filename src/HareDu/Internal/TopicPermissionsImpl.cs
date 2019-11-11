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
namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    class TopicPermissionsImpl :
        RmqBrokerClient,
        TopicPermissions
    {
        public TopicPermissionsImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<TopicPermissionsInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/topic-permissions";
            
            ResultList<TopicPermissionsInfo> result = await GetAll<TopicPermissionsInfo>(url, cancellationToken);

            return result;
        }

        public async Task<Result> Create(Action<TopicPermissionsCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new TopicPermissionsCreateActionImpl();
            action(impl);

            TopicPermissionsDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/topic-permissions/{impl.VirtualHostName.Value.SanitizeVirtualHostName()}/{impl.Username.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString()));

            Result result = await Put(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result> Delete(Action<TopicPermissionsDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new TopicPermissionsDeleteActionImpl();
            action(impl);

            string url = $"api/topic-permissions/{impl.VirtualHostName.Value.SanitizeVirtualHostName()}/{impl.Username.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, null));

            Result result = await Delete(url, cancellationToken);

            return result;
        }

        
        class TopicPermissionsDeleteActionImpl :
            TopicPermissionsDeleteAction
        {
            string _vhost;
            string _user;
            readonly List<Error> _errors;

            public Lazy<string> Username { get; }
            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }

            public TopicPermissionsDeleteActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Username = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void User(string username)
            {
                _user = username;
            
                if (string.IsNullOrWhiteSpace(_user))
                    _errors.Add(new ErrorImpl("The user is missing."));
            }

            public void VirtualHost(string name)
            {
                _vhost = name;
            
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            public void Verify()
            {
                
            }
        }


        class TopicPermissionsCreateActionImpl :
            TopicPermissionsCreateAction
        {
            string _exchange;
            string _writePattern;
            string _readPattern;
            string _vhost;
            string _user;
            readonly List<Error> _errors;

            public Lazy<TopicPermissionsDefinition> Definition { get; }
            public Lazy<string> VirtualHostName { get; }
            public Lazy<string> Username { get; }
            public Lazy<List<Error>> Errors { get; }

            public TopicPermissionsCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<TopicPermissionsDefinition>(
                    () => new TopicPermissionsDefinitionImpl(_exchange, _writePattern, _readPattern), LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                Username = new Lazy<string>(() => _user, LazyThreadSafetyMode.PublicationOnly);
            }

            public void User(string username)
            {
                _user = username;
            
                if (string.IsNullOrWhiteSpace(_user))
                    _errors.Add(new ErrorImpl("The username and/or password is missing."));
            }

            public void Configure(Action<TopicPermissionsConfiguration> configure)
            {
                var impl = new TopicPermissionsConfigurationImpl();
                configure(impl);

                _exchange = impl.ExchangeName;
                _writePattern = impl.WritePattern;
                _readPattern = impl.ReadPattern;
                
                if (string.IsNullOrWhiteSpace(_exchange))
                    _errors.Add(new ErrorImpl("Then name of the exchange is missing."));
                
                if (string.IsNullOrWhiteSpace(_writePattern))
                    _errors.Add(new ErrorImpl("The write pattern is missing."));
                
                if (string.IsNullOrWhiteSpace(_readPattern))
                    _errors.Add(new ErrorImpl("The read pattern is missing."));
            }

            public void VirtualHost(string name)
            {
                _vhost = name;

                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            public void Verify()
            {
                
            }

            
            class TopicPermissionsConfigurationImpl :
                TopicPermissionsConfiguration
            {
                public string ExchangeName { get; private set; }
                public string WritePattern { get; private set; }
                public string ReadPattern { get; private set; }

                public void OnExchange(string name) => ExchangeName = name;

                public void UsingWritePattern(string pattern) => WritePattern = pattern;

                public void UsingReadPattern(string pattern) => ReadPattern = pattern;
            }

            
            class TopicPermissionsDefinitionImpl :
                TopicPermissionsDefinition
            {
                public TopicPermissionsDefinitionImpl(string exchange, string read, string write)
                {
                    Exchange = exchange;
                    Read = read;
                    Write = write;
                }

                public string Exchange { get; }
                public string Read { get; }
                public string Write { get; }
            }
        }
    }
}
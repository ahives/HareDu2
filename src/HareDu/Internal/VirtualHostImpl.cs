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
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    class VirtualHostImpl :
        RmqBrokerClient,
        VirtualHost
    {
        public VirtualHostImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<VirtualHostInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/vhosts";
            
            ResultList<VirtualHostInfo> result = await GetAll<VirtualHostInfo>(url, cancellationToken);

            return result;
        }

        public async Task<Result> Create(Action<VirtualHostCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostCreateActionImpl();
            action(impl);

            impl.Validate();

            VirtualHostDefinition definition = impl.Definition.Value;

            string url = $"api/vhosts/{impl.VirtualHostName.Value.SanitizeVirtualHostName()}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString()));

            Result result = await Put(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result> Delete(Action<VirtualHostDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostDeleteActionImpl();
            action(impl);

            impl.Validate();

            string vHost = impl.VirtualHostName.Value.SanitizeVirtualHostName();

            string url = $"api/vhosts/{vHost}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, null));

            if (vHost == "2%f")
                return new FaultedResult(new List<Error>{ new ErrorImpl("Cannot delete the default virtual host.") }, new DebugInfoImpl(url, null));

            Result result = await Delete(url, cancellationToken);

            return result;
        }

        public async Task<Result> Startup(string vhost, Action<VirtualHostStartupAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostStartupActionImpl();
            action(impl);

            impl.Validate();

            string url = $"/api/vhosts/{vhost.SanitizeVirtualHostName()}/start/{impl.Node.Value}";

            var errors = new List<Error>();
            errors.AddRange(impl.Errors.Value);

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            
            if (errors.Any())
                return new FaultedResult(errors, new DebugInfoImpl(url, null));

            Result result = await PostEmpty(url, cancellationToken);

            return result;
        }

        
        class VirtualHostStartupActionImpl :
            VirtualHostStartupAction
        {
            string _node;
            readonly List<Error> _errors;

            public Lazy<List<Error>> Errors { get; }
            public Lazy<string> Node { get; }

            public VirtualHostStartupActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Node = new Lazy<string>(() => _node, LazyThreadSafetyMode.PublicationOnly);
            }

            public void On(string node) => _node = node;

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_node))
                    _errors.Add(new ErrorImpl("RabbitMQ node is missing."));
            }
        }


        class VirtualHostDeleteActionImpl :
            VirtualHostDeleteAction
        {
            string _vhost;
            readonly List<Error> _errors;

            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }

            public VirtualHostDeleteActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void VirtualHost(string name) => _vhost = name;

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }
        }

        
        class VirtualHostCreateActionImpl :
            VirtualHostCreateAction
        {
            bool _tracing;
            string _vhost;
            readonly List<Error> _errors;

            public Lazy<VirtualHostDefinition> Definition { get; }
            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }
            
            public VirtualHostCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<VirtualHostDefinition>(
                    () => new VirtualHostDefinitionImpl(_tracing), LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void VirtualHost(string name) => _vhost = name;

            public void Configure(Action<VirtualHostConfiguration> configuration)
            {
                var impl = new VirtualHostConfigurationImpl();
                configuration(impl);

                _tracing = impl.Tracing;
            }

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            
            class VirtualHostDefinitionImpl :
                VirtualHostDefinition
            {
                public bool Tracing { get; }

                public VirtualHostDefinitionImpl(bool tracing)
                {
                    Tracing = tracing;
                }
            }

            
            class VirtualHostConfigurationImpl :
                VirtualHostConfiguration
            {
                public bool Tracing { get; private set; }

                public void WithTracingEnabled() => Tracing = true;
            }
        }
    }
}
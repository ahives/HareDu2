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
namespace HareDu.Internal.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    internal class VirtualHostImpl :
        ResourceBase,
        VirtualHost
    {
        public VirtualHostImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<Result<IReadOnlyList<VirtualHostInfo>>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/vhosts";
            
            Result<IReadOnlyList<VirtualHostInfo>> result = await GetAll<VirtualHostInfo>(url, cancellationToken);

            return result;
        }

        public async Task<Result> Create(Action<VirtualHostCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostCreateActionImpl();
            action(impl);

            string vhost = impl.VirtualHostName.Value;

            if (string.IsNullOrWhiteSpace(vhost))
                return new FaultedResult(new List<Error> {new ErrorImpl("The name of the virtual host is missing.")});

            string url = $"api/vhosts/{SanitizeVirtualHostName(vhost)}";

            DefinedVirtualHost definition = impl.Definition.Value;

            Result result = await Put(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result> Delete(Action<VirtualHostDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostDeleteActionImpl();
            action(impl);

            string sanitizedVHost = SanitizeVirtualHostName(impl.VirtualHostName.Value);

            string url = $"api/vhosts/{sanitizedVHost}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, null));

            if (sanitizedVHost == "2%f")
                return new FaultedResult(new List<Error>{ new ErrorImpl("Cannot delete the default virtual host.") });

            Result result = await Delete(url, cancellationToken);

            return result;
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

            public void VirtualHost(string name)
            {
                _vhost = name;

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

            public Lazy<DefinedVirtualHost> Definition { get; }
            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }
            
            public VirtualHostCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<DefinedVirtualHost>(
                    () => new DefinedVirtualHostImpl(_tracing), LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void VirtualHost(string name)
            {
                _vhost = name;

                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            public void Configure(Action<VirtualHostConfiguration> configuration)
            {
                var impl = new VirtualHostConfigurationImpl();
                configuration(impl);

                _tracing = impl.Tracing;
            }

            
            class DefinedVirtualHostImpl :
                DefinedVirtualHost
            {
                public bool Tracing { get; }

                public DefinedVirtualHostImpl(bool tracing)
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
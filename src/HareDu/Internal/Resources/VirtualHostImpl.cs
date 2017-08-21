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

    internal class VirtualHostImpl :
        ResourceBase,
        VirtualHost
    {
        public VirtualHostImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<VirtualHostInfo>>> GetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/vhosts";

            LogInfo("Sent request to return all information on all virtual hosts on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<VirtualHostInfo>> result = await response.GetResponse<IEnumerable<VirtualHostInfo>>();

            return result;
        }

        public async Task<Result> Create(Action<VirtualHostDefinition> definition, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new VirtualHostDefinitionImpl();
            definition(impl);

            if (string.IsNullOrWhiteSpace(impl.VirtualHost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = impl.VirtualHost.SanitizeVirtualHostName();

            string url = $"api/vhosts/{sanitizedVHost}";

            VirtualHostSettings settings = impl.Settings.Value;

            HttpResponseMessage response = await HttpPut(url, settings, cancellationToken);
            Result result = response.GetResponse();
            
            LogInfo($"Sent request to RabbitMQ server to create virtual host '{sanitizedVHost}'.");

            return result;
        }

        public async Task<Result> Create(string vhost, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/vhosts/{sanitizedVHost}";

            LogInfo($"Sent request to RabbitMQ server to create virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpPut(url, new StringContent(string.Empty), cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string vhost, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();
            
            if (sanitizedVHost == "2%f")
                throw new DeleteVirtualHostException("Cannot delete the default virtual host.");

            string url = $"api/vhosts/{sanitizedVHost}";

            LogInfo($"Sent request to RabbitMQ server to delete virtual host '{vhost}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        
        class VirtualHostDefinitionImpl :
            VirtualHostDefinition
        {
            static bool _tracing;
            
            public Lazy<VirtualHostSettings> Settings { get; }
            public string VirtualHost { get; private set; }
            
            public VirtualHostDefinitionImpl() => Settings = new Lazy<VirtualHostSettings>(Init, LazyThreadSafetyMode.PublicationOnly);

            VirtualHostSettings Init() => new VirtualHostSettingsImpl(_tracing);

            public void Configure(Action<VirtualHostConfiguration> configuration)
            {
                var impl = new VirtualHostConfigurationImpl();
                configuration(impl);

                _tracing = impl.Tracing;
                VirtualHost = impl.VirtualHost;
            }

            
            class VirtualHostSettingsImpl :
                VirtualHostSettings
            {
                public VirtualHostSettingsImpl(bool tracing)
                {
                    Tracing = tracing;
                }

                public bool Tracing { get; }
            }

            
            class VirtualHostConfigurationImpl :
                VirtualHostConfiguration
            {
                public string VirtualHost { get; private set; }
                public bool Tracing { get; private set; }
                
                public void Name(string name) => VirtualHost = name;

                public void EnableTracing() => Tracing = true;
            }
        }
    }
}
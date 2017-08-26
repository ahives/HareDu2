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

        public async Task<Result<IEnumerable<VirtualHostInfo>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/vhosts";

            LogInfo("Sent request to return all information on all virtual hosts on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<VirtualHostInfo>> result = await response.GetResponse<IEnumerable<VirtualHostInfo>>();

            return result;
        }

        public async Task<Result> CreateAsync(Action<VirtualHostCreateAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new VirtualHostCreateActionImpl();
            action(impl);

            string vhost = impl.VirtualHostName.Value;

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/vhosts/{sanitizedVHost}";

            VirtualHostSettings settings = impl.Settings.Value;

            HttpResponseMessage response = await HttpPut(url, settings, cancellationToken);
            Result result = response.GetResponse();
            
            LogInfo($"Sent request to RabbitMQ server to create virtual host '{sanitizedVHost}'.");

            return result;
        }

        public async Task<Result> DeleteAsync(Action<VirtualHostDeleteAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new VirtualHostDeleteActionImpl();
            action(impl);
            
            if (string.IsNullOrWhiteSpace(impl.VirtualHostName))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = impl.VirtualHostName.SanitizeVirtualHostName();
            
            if (sanitizedVHost == "2%f")
                throw new DeleteVirtualHostException("Cannot delete the default virtual host.");

            string url = $"api/vhosts/{sanitizedVHost}";

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to delete virtual host '{impl.VirtualHostName}'.");

            return result;
        }

        
        class VirtualHostDeleteActionImpl :
            VirtualHostDeleteAction
        {
            public string VirtualHostName { get; private set; }
            
            public void VirtualHost(string vhost) => VirtualHostName = vhost;
        }

        
        class VirtualHostCreateActionImpl :
            VirtualHostCreateAction
        {
            static bool _tracing;
            static string _vhost;

            public Lazy<VirtualHostSettings> Settings { get; }
            public Lazy<string> VirtualHostName { get; }
            
            public VirtualHostCreateActionImpl()
            {
                Settings = new Lazy<VirtualHostSettings>(
                    () => new VirtualHostSettingsImpl(_tracing), LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void VirtualHost(string name) => _vhost = name;

            public void Configure(Action<VirtualHostConfiguration> configuration)
            {
                var impl = new VirtualHostConfigurationImpl();
                configuration(impl);

                _tracing = impl.Tracing;
            }

            
            class VirtualHostSettingsImpl :
                VirtualHostSettings
            {
                public bool Tracing { get; }
                
                public VirtualHostSettingsImpl(bool tracing) => Tracing = tracing;
            }

            
            class VirtualHostConfigurationImpl :
                VirtualHostConfiguration
            {
                public bool Tracing { get; private set; }

                public void EnableTracing() => Tracing = true;
            }
        }
    }
}
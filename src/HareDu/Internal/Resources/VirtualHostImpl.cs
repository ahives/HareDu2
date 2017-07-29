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
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Logging;
    using Exceptions;
    using Model;

    internal class VirtualHostImpl :
        ResourceBase,
        VirtualHost
    {
        public VirtualHostImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }
        
        public async Task<Result<VirtualHostHealthCheck>> IsHealthy(string vhost, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string sanitizedVHostName = vhost.SanitizeVirtualHostName();
            
            string url = $"api/aliveness-test/{sanitizedVHostName}";

            LogInfo($"Sent request to execute an aliveness test on virtual host '{sanitizedVHostName}' on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<VirtualHostHealthCheck> result = await response.GetResponse<VirtualHostHealthCheck>();

            return result;
        }

        public async Task<Result<VirtualHostDefinition>> GetDefinition(string vhost, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/definitions/{sanitizedVHost}";

            LogInfo($"Sent request to return all information corresponding to virtual host '{sanitizedVHost}' on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<VirtualHostDefinition> result = await response.GetResponse<VirtualHostDefinition>();

            return result;
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

        public async Task<Result> Create(string vhost, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

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

            string sanitizedVHost = vhost.SanitizeVirtualHostName();
            
            if (sanitizedVHost == "2%f")
                throw new DeleteVirtualHostException("Cannot delete the default virtual host.");

            string url = $"api/vhosts/{sanitizedVHost}";

            LogInfo($"Sent request to RabbitMQ server to delete virtual host '{vhost}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }
    }
}
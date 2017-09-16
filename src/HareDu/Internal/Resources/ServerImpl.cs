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
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Model;

    internal class ServerImpl :
        ResourceBase,
        Server
    {
        public ServerImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<ServerHealth>> CheckUpAsync(Action<ServerHealthCheckConstraints> constraints, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new ServerHealthCheckConstraintsImpl();
            constraints(impl);

            string url;
            
            switch (impl.CheckUpType)
            {
                case HealthCheckType.VirtualHost:
                    string sanitizedVHostName = impl.ResourceName.SanitizeVirtualHostName();
            
                    url = $"api/aliveness-test/{sanitizedVHostName}";

                    LogInfo($"Sent request to execute an aliveness test on virtual host '{sanitizedVHostName}' on current RabbitMQ server.");
                    break;
                    
                case HealthCheckType.Node:
                    url = $"api/healthchecks/node/{impl.ResourceName}";

                    LogInfo($"Sent request to execute an health check on RabbitMQ server node '{impl.ResourceName}'.");
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }

            HttpResponseMessage response = await PerformHttpGet(url, cancellationToken);
            Result<ServerHealth> result = await response.DeserializeResponse<ServerHealth>();

            return result;
        }

        
        class ServerHealthCheckConstraintsImpl :
            ServerHealthCheckConstraints
        {
            public string ResourceName { get; private set; }
            public HealthCheckType CheckUpType { get; private set; }
            
            public void Name(string name) => ResourceName = name;

            public void Type(HealthCheckType type) => CheckUpType = type;
        }
    }
}
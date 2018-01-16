// Copyright 2013-2018 Albert L. Hives
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
    using Model;

    internal class ServerImpl :
        ResourceBase,
        Server
    {
        public ServerImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<Result<ServerHealth>> HealthCheck(Action<HealthCheckAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new HealthCheckActionImpl();
            action(impl);

            string url;
            
            switch (impl.CheckUpType)
            {
                case HealthCheckType.VirtualHost:
                    string sanitizedVHostName = impl.ResourceName.SanitizeVirtualHostName();
            
                    url = $"api/aliveness-test/{sanitizedVHostName}";
                    break;
                    
                case HealthCheckType.Node:
                    url = $"api/healthchecks/node/{impl.ResourceName}";
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var result = await Get<ServerHealth>(url, cancellationToken);

            return result;
        }

        
        class HealthCheckActionImpl :
            HealthCheckAction
        {
            public string ResourceName { get; private set; }
            public HealthCheckType CheckUpType { get; private set; }
            
            public void VirtualHost(string name)
            {
                CheckUpType = HealthCheckType.VirtualHost;
                ResourceName = name;
            }

            public void Node(string name)
            {
                CheckUpType = HealthCheckType.Node;
                ResourceName = name;
            }
        }
    }
}
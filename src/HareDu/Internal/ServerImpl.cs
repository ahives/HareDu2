// Copyright 2013-2020 Albert L. Hives
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
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    class ServerImpl :
        BaseBrokerObject,
        Server
    {
        public ServerImpl(HttpClient client)
            : base(client)
        {
        }

        public Task<Result<ServerInfo>> Get(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/definitions";
            
            return Get<ServerInfo>(url, cancellationToken);
        }

        public Task<Result<ServerHealthInfo>> GetHealth(Action<HealthCheckAction> action,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new HealthCheckActionImpl();
            action(impl);

            string url;
            
            switch (impl.CheckUpType)
            {
                case HealthCheckType.VirtualHost:
                    url = $"api/aliveness-test/{impl.RmqObjectName.ToSanitizedName()}";
                    break;
                    
                case HealthCheckType.Node:
                    url = $"api/healthchecks/node/{impl.RmqObjectName}";
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Get<ServerHealthInfo>(url, cancellationToken);
        }

        
        class HealthCheckActionImpl :
            HealthCheckAction
        {
            public string RmqObjectName { get; private set; }
            public HealthCheckType CheckUpType { get; private set; }
            
            public void VirtualHost(string name)
            {
                CheckUpType = HealthCheckType.VirtualHost;
                RmqObjectName = name;
            }

            public void Node(string name)
            {
                CheckUpType = HealthCheckType.Node;
                RmqObjectName = name;
            }
        }
    }
}
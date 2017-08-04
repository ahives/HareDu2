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
    using Model;

    internal class NodeImpl :
        ResourceBase,
        Node
    {
        public NodeImpl(HttpClient client, ILog logger, int retryLimit)
            : base(client, logger, retryLimit)
        {
        }
        
        public async Task<Result<NodeHealthCheck>> IsHealthy(string node, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);
            
            string url = $"api/healthchecks/node/{node}";

            LogInfo($"Sent request to execute an health check on RabbitMQ server node '{node}'.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<NodeHealthCheck> result = await response.GetResponse<NodeHealthCheck>();

            return result;
        }

        public async Task<Result<IEnumerable<ChannelInfo>>> GetChannels(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/nodes";

            LogInfo("Sent request to return all information on all nodes on current RabbitMQ cluster.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<ChannelInfo>> result = await response.GetResponse<IEnumerable<ChannelInfo>>();

            return result;
        }

        public async Task<Result<IEnumerable<ConnectionInfo>>> GetConnections(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/connections";

            LogInfo($"Sent request to return all connection information on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<ConnectionInfo>> result = await response.GetResponse<IEnumerable<ConnectionInfo>>();

            return result;
        }

        public async Task<Result<IEnumerable<ConsumerInfo>>> GetConsumers(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/consumers";

            LogInfo($"Sent request to return all consumer information on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<ConsumerInfo>> result = await response.GetResponse<IEnumerable<ConsumerInfo>>();

            return result;
        }
    }
}
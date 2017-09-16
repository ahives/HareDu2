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
    using Configuration;
    using Model;

    internal class NodeImpl :
        ResourceBase,
        Node
    {
        public NodeImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<ChannelInfo>>> GetChannelsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/nodes";

            HttpResponseMessage response = await PerformHttpGet(url, cancellationToken);
            Result<IEnumerable<ChannelInfo>> result = await response.DeserializeResponse<IEnumerable<ChannelInfo>>();

            LogInfo("Sent request to return all information on all nodes on current RabbitMQ cluster.");

            return result;
        }

        public async Task<Result<IEnumerable<ConnectionInfo>>> GetConnectionsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/connections";

            HttpResponseMessage response = await PerformHttpGet(url, cancellationToken);
            Result<IEnumerable<ConnectionInfo>> result = await response.DeserializeResponse<IEnumerable<ConnectionInfo>>();

            LogInfo($"Sent request to return all connection information on current RabbitMQ server.");

            return result;
        }

        public async Task<Result<IEnumerable<ConsumerInfo>>> GetConsumersAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/consumers";

            HttpResponseMessage response = await PerformHttpGet(url, cancellationToken);
            Result<IEnumerable<ConsumerInfo>> result = await response.DeserializeResponse<IEnumerable<ConsumerInfo>>();

            LogInfo($"Sent request to return all consumer information on current RabbitMQ server.");

            return result;
        }

        public async Task<Result<ServerDefinition>> GetDefintiions(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/definitions";

            HttpResponseMessage response = await PerformHttpGet(url, cancellationToken);
            Result<ServerDefinition> result = await response.DeserializeResponse<ServerDefinition>();

            LogInfo($"Sent request to return all node object definitions on current RabbitMQ server.");

            return result;
        }
    }
}
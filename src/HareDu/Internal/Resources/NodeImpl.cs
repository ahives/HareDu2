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

        public async Task<Result<IEnumerable<ChannelInfo>>> GetChannelsAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/nodes";
            var result = await Get<IEnumerable<ChannelInfo>>(url, cancellationToken);

            return result;
        }

        public async Task<Result<IEnumerable<ConnectionInfo>>> GetConnectionsAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/connections";
            var result = await Get<IEnumerable<ConnectionInfo>>(url, cancellationToken);

            return result;
        }

        public async Task<Result<IEnumerable<ConsumerInfo>>> GetConsumersAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/consumers";
            var result = await Get<IEnumerable<ConsumerInfo>>(url, cancellationToken);

            return result;
        }

        public async Task<Result<ServerDefinition>> GetDefintiions(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/definitions";
            var result = await Get<ServerDefinition>(url, cancellationToken);

            return result;
        }
    }
}
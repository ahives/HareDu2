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
    using Model;

    internal class ClusterImpl :
        ResourceBase,
        Cluster
    {
        public ClusterImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<ClusterInfo>> GetDetails(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/overview";

            LogInfo("Sent request to return information pertaining to the RabbitMQ cluster.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<ClusterInfo> result = await response.GetResponse<ClusterInfo>();

            return result;
        }

        public async Task<Result<IEnumerable<NodeInfo>>> GetNodes(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/nodes";

            LogInfo("Sent request to return all information on all nodes on current RabbitMQ cluster.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<NodeInfo>> result = await response.GetResponse<IEnumerable<NodeInfo>>();

            return result;
        }
    }
}
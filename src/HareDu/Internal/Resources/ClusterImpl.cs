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
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Model;

    internal class ClusterImpl :
        ResourceBase,
        Cluster
    {
        public ClusterImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<ClusterInfo>> GetDetails(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/overview";
            var result = await Get<ClusterInfo>(url, cancellationToken);

            return result;
        }

        public async Task<Result<IEnumerable<NodeInfo>>> GetNodes(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/nodes";
            var result = await Get<IEnumerable<NodeInfo>>(url, cancellationToken);

            return result;
        }
    }
}
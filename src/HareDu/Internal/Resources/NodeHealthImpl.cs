// Copyright 2013-2019 Albert L. Hives
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
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    class NodeHealthImpl :
        HttpClientHelper,
        NodeHealth
    {
        public NodeHealthImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<Result<NodeHealthInfo>> GetDetails(string node = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = string.IsNullOrWhiteSpace(node) ? "api/healthchecks/node" : $"/api/healthchecks/node/{node}";

            Result<NodeHealthInfo> result = await Get<NodeHealthInfo>(url, cancellationToken);

            return result;
        }
    }
}
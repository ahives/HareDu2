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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    class NodeImpl :
        BaseBrokerObject,
        Node
    {
        public NodeImpl(HttpClient client)
            : base(client)
        {
        }

        public Task<ResultList<NodeInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/nodes";
            
            return GetAll<NodeInfo>(url, cancellationToken);
        }

        public Task<Result<NodeHealthInfo>> GetHealth(string node = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = string.IsNullOrWhiteSpace(node) ? "api/healthchecks/node" : $"/api/healthchecks/node/{node}";

            return Get<NodeHealthInfo>(url, cancellationToken);
        }

        public Task<Result<NodeMemoryUsageInfo>> GetMemoryUsage(string node,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(node))
                errors.Add(new ErrorImpl("Name of the node for which to return memory usage data is missing."));
            
            string url = $"api/nodes/{node}/memory";
            
            if (errors.Any())
                return Task.FromResult<Result<NodeMemoryUsageInfo>>(new FaultedResult<NodeMemoryUsageInfo>(errors, new DebugInfoImpl(url)));
            
            return Get<NodeMemoryUsageInfo>(url, cancellationToken);
        }
    }
}
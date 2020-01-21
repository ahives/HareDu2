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
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public class MemoryImpl :
        BaseBrokerObject,
        Memory
    {
        public MemoryImpl(HttpClient client)
            : base(client)
        {
        }
        
        public async Task<Result<NodeMemoryUsageInfo>> GetDetails(string node, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = $"api/nodes/{node}/memory";
            
            Result<NodeMemoryUsageInfo> result = await Get<NodeMemoryUsageInfo>(url, cancellationToken);

            return result;
        }
    }
}
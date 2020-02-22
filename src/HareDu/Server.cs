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
namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface Server :
        BrokerObject
    {
        /// <summary>
        /// Returns all object definitions on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ServerInfo>> Get(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Perform a health check on a virtual host or node.
        /// </summary>
        /// <param name="action">Constraints of how the </param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<Result<ServerHealthInfo>> GetHealth(Action<HealthCheckAction> action, CancellationToken cancellationToken = default);
    }
}
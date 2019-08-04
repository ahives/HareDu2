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
namespace HareDu.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface Exchange :
        Resource
    {
        /// <summary>
        /// Returns all exchanges on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<ResultList<ExchangeInfo>> GetAll(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates the specified exchange on the target virtual host.
        /// </summary>
        /// <param name="action">Describes how the queue will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Create(Action<ExchangeCreateAction> action, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete the specified exchange on the target virtual host.
        /// </summary>
        /// <param name="action">Describes how the queue will be deleted.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Delete(Action<ExchangeDeleteAction> action, CancellationToken cancellationToken = default);
    }
}
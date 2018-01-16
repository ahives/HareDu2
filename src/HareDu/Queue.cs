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
namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface Queue :
        Resource
    {
        /// <summary>
        /// Returns all queues on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<IEnumerable<QueueInfo>>> GetAll(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates the specified queue on the target virtual host and perhaps RabbitMQ node.
        /// </summary>
        /// <param name="action">Describes how the queue will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<QueueInfo>> Create(Action<QueueCreateAction> action, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete the specified queue on the target virtual host and perhaps RabbitMQ node.
        /// </summary>
        /// <param name="action">Describes how the queue will be deleted.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<QueueInfo>> Delete(Action<QueueDeleteAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Purge all messages in the specified queue on the target virtual host on the current RAbbitMQ node.
        /// </summary>
        /// <param name="action">Describes how the queue will be purged.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<QueueInfo>> Empty(Action<QueueEmptyAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Pull a specified number of messages from the specified queue on the target virtual host on the current RAbbitMQ node.
        /// </summary>
        /// <param name="action">Describes how the queue will be purged.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<QueueInfo>> Peek(Action<QueuePeekAction> action, CancellationToken cancellationToken = default);
    }
}
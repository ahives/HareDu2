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

    public interface ScopedParameter :
        Resource
    {
        /// <summary>
        /// Returns all scoped parameters on the current RabbitMQ server.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<IEnumerable<ScopedParameterInfo>>> GetAll(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates a scoped parameter for a particular component and virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action">Describes how the scoped parameter will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<ScopedParameterInfo>> Create(Action<ScopedParameterCreateAction> action, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete the specified scoped parameter for a particular component and virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action">Describes how the scoped parameter will be delete.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result<ScopedParameterInfo>> Delete(Action<ScopedParameterDeleteAction> action, CancellationToken cancellationToken = default);
    }
}
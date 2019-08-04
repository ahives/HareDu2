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

    public interface User :
        Resource
    {
        /// <summary>
        /// Returns information about all users on the current RabbitMQ server.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<ResultList<UserInfo>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns information about all users that do not have permissions on the current RabbitMQ server.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ResultList<UserInfo>> GetAllWithoutPermissions(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a user on the current RabbitMQ server.
        /// </summary>
        /// <param name="action">Describes how the user permission will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Create(Action<UserCreateAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the specified user on the current RabbitMQ server.
        /// </summary>
        /// <param name="action">Describes how the virtual host will be delete.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Delete(Action<UserDeleteAction> action, CancellationToken cancellationToken = default);
    }
}
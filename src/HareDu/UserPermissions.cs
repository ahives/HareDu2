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
namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface UserPermissions :
        Resource
    {
        /// <summary>
        /// Returns information about all user permissions on the current RabbitMQ server.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<Result<IEnumerable<UserPermissionsInfo>>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a user permission and assign it to a user on a specific virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action">Describes how the user permission will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<Result<UserPermissionsInfo>> Create(Action<UserPermissionsCreateAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the specified user permission assigned to a specified user on a specific virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action">Describes how the virtual host will be delete.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<Result<UserPermissionsInfo>> Delete(Action<UserPermissionsDeleteAction> action, CancellationToken cancellationToken = default);
    }
}
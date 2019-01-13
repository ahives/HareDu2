﻿// Copyright 2013-2019 Albert L. Hives
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

    public interface VirtualHost :
        Resource
    {
        /// <summary>
        /// Returns information about each virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<Result<IReadOnlyList<VirtualHostInfo>>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns limit information about each virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IReadOnlyList<VirtualHostLimits>>> GetAllLimits(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action">Describes how the virtual host will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<Result> Create(Action<VirtualHostCreateAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Defines specified limits on the virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> DefineLimits(Action<VirtualHostConfigureLimitsAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the specified virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action">Describes how the virtual host will be delete.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<Result> Delete(Action<VirtualHostDeleteAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the limits for the specified virtual host on the current RabbitMQ server.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> DeleteLimits(Action<VirtualHostDeleteLimitsAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vhost"></param>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> Startup(string vhost, Action<VirtualHostStartupAction> action, CancellationToken cancellationToken = default);
    }
}
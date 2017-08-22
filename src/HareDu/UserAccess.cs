﻿// Copyright 2013-2017 Albert L. Hives
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

    public interface UserAccess :
        Resource
    {
        Task<Result<IEnumerable<UserAccessInfo>>> GetAll(CancellationToken cancellationToken = default(CancellationToken));

        Task<Result> Create(Action<UserAccessDefinition> definition, CancellationToken cancellationToken = default(CancellationToken));

        Task<Result> Delete(string vhost, string username, CancellationToken cancellationToken = default(CancellationToken));
    }
}
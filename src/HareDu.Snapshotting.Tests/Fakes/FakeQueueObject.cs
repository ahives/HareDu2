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
namespace HareDu.Snapshotting.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using HareDu.Model;

    public class FakeQueueObject :
        Queue
    {
        public async Task<ResultList<QueueInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            QueueInfo channel = new FakeQueueInfo();

            List<QueueInfo> data = new List<QueueInfo> {channel};

            return new SuccessfulResultList<QueueInfo>(data, null);
        }

        public async Task<Result> Create(Action<QueueCreateAction> action, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async Task<Result> Delete(Action<QueueDeleteAction> action, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async Task<Result> Empty(Action<QueueEmptyAction> action, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async Task<Result<QueueInfo>> Peek(Action<QueuePeekAction> action, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
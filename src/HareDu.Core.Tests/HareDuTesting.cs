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
namespace HareDu.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fakes;

    public class HareDuTesting
    {
        protected Result<FakeObject> GetResult(Guid id, bool hasData) => new FakeResult<FakeObject>(Get(id), hasData);

        protected Result<FakeObject> GetResult() => new FakeResult<FakeObject>();

        protected Task<Result<FakeObject>> GetResultAsync(Guid id, bool hasData) =>
            Task.FromResult<Result<FakeObject>>(new FakeResult<FakeObject>(Get(id), hasData));

        protected Task<Result<FakeObject>> GetResultAsync() =>
            Task.FromResult<Result<FakeObject>>(new FakeResult<FakeObject>());

        protected Task<Result<FakeObject>> GetNullResultAsync() => Task.FromResult<Result<FakeObject>>(null);

        protected Task<ResultList<FakeObject>> GetResultListAsync(bool hasData, params Guid[] identifiers) =>
            Task.FromResult<ResultList<FakeObject>>(
                new FakeResultList<FakeObject>(GetAll(identifiers).ToList(), hasData));

        protected Task<ResultList<FakeObject>> GetNullResultListAsync() => Task.FromResult<ResultList<FakeObject>>(null);

        protected ResultList<FakeObject> GetNullResultList() => null;

        protected ResultList<FakeObject> GetResultList(bool hasData, params Guid[] identifiers) =>
            new FakeResultList<FakeObject>(GetAll(identifiers).ToList(), hasData);

        protected FakeObject Get(Guid id) => new FakeObjectImpl(id);

        IEnumerable<FakeObject> GetAll(params Guid[] identifiers)
        {
            foreach (var id in identifiers)
            {
                yield return new FakeObjectImpl(id);
            }
        }


        class FakeResultList<T> :
            ResultList<T>
        {
            public FakeResultList(IReadOnlyList<T> data, bool hasData)
            {
                Data = data;
                HasFaulted = false;
                HasData = hasData;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<T> Data { get; }
            public bool HasData { get; }
        }
        
        
        class FakeResult<T> :
            Result<T>
        {
            public FakeResult(T data, bool hasData)
            {
                Data = data;
                HasData = hasData;
                HasFaulted = false;
            }

            public FakeResult()
            {
                HasData = false;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public T Data { get; }
            public bool HasData { get; }
        }
        
        
        class FakeObjectImpl :
            FakeObject
        {
            public FakeObjectImpl(Guid id)
            {
                Id = id;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public Guid Id { get; }
            public DateTimeOffset Timestamp { get; }
        }
    }
}
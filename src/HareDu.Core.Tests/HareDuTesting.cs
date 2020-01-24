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
        protected Result<FakeObject> GetResult(Guid id) =>
            new SuccessfulResult<FakeObject>(Get(id), null);

        protected Task<Result<FakeObject>> GetResultAsync(Guid id1) =>
            Task.FromResult<Result<FakeObject>>(
                new SuccessfulResult<FakeObject>(Get(id1), null));

        protected Task<ResultList<FakeObject>> GetResultListAsync(params Guid[] identifiers) =>
            Task.FromResult<ResultList<FakeObject>>(
                new SuccessfulResultList<FakeObject>(GetAll(identifiers).ToList(), null));

        protected Task<ResultList<FakeObject>> GetNullResultListAsync() =>
            Task.FromResult<ResultList<FakeObject>>(null);

        protected ResultList<FakeObject> GetNullResultList() => null;

        protected ResultList<FakeObject> GetResultList(params Guid[] identifiers) =>
            new SuccessfulResultList<FakeObject>(GetAll(identifiers).ToList(), null);

        protected FakeObject Get(Guid id) => new FakeObjectImpl(id);

        IEnumerable<FakeObject> GetAll(params Guid[] identifiers)
        {
            foreach (var id in identifiers)
            {
                yield return new FakeObjectImpl(id);
            }
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
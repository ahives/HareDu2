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
namespace HareDu.Core.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Fakes;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ProjectionExtensionsTests
    {
        [Test]
        public void Verify_can_select_data_list_async()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();

            var data = GetResultListAsync(id1, id2, id3).Select(x => x.Data);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(3);
            data[0].Id.ShouldBe(id1);
            data[1].Id.ShouldBe(id2);
            data[2].Id.ShouldBe(id3);
        }

        [Test]
        public void Verify_can_select_data_list()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();

            var data = GetResultList(id1, id2, id3).Select(x => x.Data);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(3);
            data[0].Id.ShouldBe(id1);
            data[1].Id.ShouldBe(id2);
            data[2].Id.ShouldBe(id3);
        }

        [Test]
        public void Verify_can_select_data_async()
        {
            Guid id = Guid.NewGuid();

            var data = GetResultAsync(id).Select(x => x.Data);
            
            data.ShouldNotBeNull();
            data.Id.ShouldBe(id);
        }

        [Test]
        public void Verify_can_select_data()
        {
            Guid id = Guid.NewGuid();

            var data = GetResult(id).Select(x => x.Data);
            
            data.ShouldNotBeNull();
            data.Id.ShouldBe(id);
        }

        Result<FakeObject> GetResult(Guid id1) =>
            new SuccessfulResult<FakeObject>(Get(id1), null);

        Task<Result<FakeObject>> GetResultAsync(Guid id1) =>
            Task.FromResult<Result<FakeObject>>(
                new SuccessfulResult<FakeObject>(Get(id1), null));

        Task<ResultList<FakeObject>> GetResultListAsync(Guid id1, Guid id2, Guid id3) =>
            Task.FromResult<ResultList<FakeObject>>(
                new SuccessfulResultList<FakeObject>(GetAll(id1, id2, id3).ToList(), null));

        ResultList<FakeObject> GetResultList(Guid id1, Guid id2, Guid id3) =>
                new SuccessfulResultList<FakeObject>(GetAll(id1, id2, id3).ToList(), null);

        FakeObject Get(Guid id) => new FakeObjectImpl(id);

        IEnumerable<FakeObject> GetAll(Guid id1, Guid id2, Guid id3)
        {
            yield return new FakeObjectImpl(id1);
            yield return new FakeObjectImpl(id2);
            yield return new FakeObjectImpl(id3);
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
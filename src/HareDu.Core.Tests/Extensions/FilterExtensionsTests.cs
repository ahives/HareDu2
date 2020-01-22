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
    public class FilterExtensionsTests
    {
        [Test]
        public void Verify_can_select_data_list_async_1()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();

            var data = GetResultListAsync(id2, id1, id2).Where(x => x.Id == id2);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(2);
            data[0].Id.ShouldBe(id2);
            data[1].Id.ShouldBe(id2);
        }

        [Test]
        public void Verify_can_select_data_list_async_2()
        {
            Guid id = Guid.NewGuid();

            var data = GetResultListAsync().Where(x => x.Id == id);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(0);
        }

        [Test]
        public void Verify_can_select_data_list_1()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();

            var data = GetResultList(id2, id1, id2).Where(x => x.Id == id2);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(2);
            data[0].Id.ShouldBe(id2);
            data[1].Id.ShouldBe(id2);
        }

        [Test]
        public void Verify_can_select_data_list_2()
        {
            Guid id = Guid.NewGuid();

            var data = GetResultList().Where(x => x.Id == id);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(0);
        }

        Task<ResultList<FakeObject>> GetResultListAsync() =>
            Task.FromResult<ResultList<FakeObject>>(
                new SuccessfulResultList<FakeObject>(GetAllEmpty().ToList(), null));

        Task<ResultList<FakeObject>> GetResultListAsync(Guid id1, Guid id2, Guid id3) =>
            Task.FromResult<ResultList<FakeObject>>(
                new SuccessfulResultList<FakeObject>(GetAll(id1, id2, id3).ToList(), null));

        ResultList<FakeObject> GetResultList(Guid id1, Guid id2, Guid id3) =>
            new SuccessfulResultList<FakeObject>(GetAll(id1, id2, id3).ToList(), null);

        ResultList<FakeObject> GetResultList() =>
            new SuccessfulResultList<FakeObject>(GetAllEmpty().ToList(), null);

        IEnumerable<FakeObject> GetAll(Guid id1, Guid id2, Guid id3)
        {
            yield return new FakeObjectImpl(id1);
            yield return new FakeObjectImpl(id2);
            yield return new FakeObjectImpl(id3);
        }

        IEnumerable<FakeObject> GetAllEmpty()
        {
            yield break;
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
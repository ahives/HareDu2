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
    public class LinqExtensionsTests
    {
        [Test]
        public void Verify_Any_works()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();

            bool found = Get(id1, id2, id3).Any();
            
            found.ShouldBeTrue();
        }

        [Test]
        public void Verify_Any_with_predicate_works()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();

            bool found = Get(id1, id2, id3).Any(x => x.Id == id2);
            
            found.ShouldBeTrue();
        }
        
        [Test]
        public void Verify_FirstOrDefault_works()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            
            var result = Get(id1, id2, id3).FirstOrDefault();
 
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id1);
        }

        Task<ResultList<FakeObject>> Get(Guid id1, Guid id2, Guid id3) =>
            Task.FromResult<ResultList<FakeObject>>(
                new SuccessfulResultList<FakeObject>(GetAll(id1, id2, id3).ToList(), null));

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
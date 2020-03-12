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
    public class TaskExtensionsTests
    {
        [Test]
        public void Verify_Unfold_works()
        {
            var result = Get().GetResult();
            
            result.HasData.ShouldBeTrue();
            result.HasFaulted.ShouldBeFalse();
            result.Data.Count.ShouldBe(3);
        }

        Task<ResultList<FakeObject>> Get() =>
            Task.FromResult<ResultList<FakeObject>>(
                new SuccessfulResultList<FakeObject>(GetAll().ToList(), null));

        IEnumerable<FakeObject> GetAll()
        {
            yield return new FakeObjectImpl();
            yield return new FakeObjectImpl();
            yield return new FakeObjectImpl();
        }

        
        class FakeObjectImpl :
            FakeObject
        {
            public FakeObjectImpl()
            {
                Id = Guid.NewGuid();
                Timestamp = DateTimeOffset.UtcNow;
            }

            public Guid Id { get; }
            public DateTimeOffset Timestamp { get; }
        }
    }
}
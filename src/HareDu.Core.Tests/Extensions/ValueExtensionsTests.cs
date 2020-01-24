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
    using Core.Extensions;
    using Fakes;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ValueExtensionsTests :
        HareDuTesting
    {
        [Test]
        public void Verify_can_get_value_from_result_list()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            
            var result = GetResultListAsync(id1, id2, id3).Unfold();
            
            result.TryGetValue(1, out FakeObject value).ShouldBeTrue();
            value.Id.ShouldBe(id2);
        }

        [Test]
        public void Verify_cannot_get_value_from_result_list()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            
            var result = GetResultListAsync(id1, id2, id3).Unfold();
            
            result.TryGetValue(100, out FakeObject value).ShouldBeFalse();
        }

        [Test]
        public void Verify_object_is_not_null()
        {
            var result = Get(Guid.NewGuid());
            
            result.IsNull().ShouldBeFalse();
        }

        [Test]
        public void Verify_object_is_null()
        {
            FakeObject result = null;
            
            result.IsNull().ShouldBeTrue();
        }
    }
}
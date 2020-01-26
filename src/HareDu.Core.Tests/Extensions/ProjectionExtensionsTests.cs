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
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ProjectionExtensionsTests :
        HareDuTesting
    {
        [Test]
        public void Verify_can_select_list_data_async()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();

            var data = GetResultListAsync(true, id1, id2, id3).Select(x => x.Data);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(3);
            data[0].Id.ShouldBe(id1);
            data[1].Id.ShouldBe(id2);
            data[2].Id.ShouldBe(id3);
        }

        [Test]
        public void Verify_select_empty_list_will_not_throw_async()
        {
            var data = GetResultListAsync(false).Select(x => x.Data);
            
            data.ShouldBe(default);
        }
        
        [Test]
        public void Verify_will_not_throw_on_select_data_null_async()
        {
            var data = GetNullResultListAsync().Select(x => x.Data);
            
            data.ShouldBe(default);
        }

        [Test]
        public void Verify_can_select_list_data()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();

            var data = GetResultList(true, id1, id2, id3).Select(x => x.Data);
            
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

            var data = GetResultAsync(id, true).Select(x => x.Data);
            
            data.ShouldNotBeNull();
            data.Id.ShouldBe(id);
        }

        [Test]
        public void Verify_will_not_throw_when_data_missing_async()
        {
            var data = GetResultAsync(Guid.NewGuid(), false).Select(x => x.Data);
            
            data.ShouldBe(default);
        }

        [Test]
        public void Verify_will_not_throw_on_select_when_data_null_async()
        {
            var data = GetNullResultAsync().Select(x => x.Data);
            
            data.ShouldBeNull();
        }

        [Test]
        public void Verify_will_not_throw_when_result_null_async()
        {
            var data = GetNullResultAsync().Select(x => x.Data);
            
            data.ShouldBeNull();
        }

        [Test]
        public void Verify_can_select_data()
        {
            Guid id = Guid.NewGuid();

            var data = GetResult(id, true).Select(x => x.Data);
            
            data.ShouldNotBeNull();
            data.Id.ShouldBe(id);
        }

        [Test]
        public void Verify_will_not_throw_when_no_data_present_1()
        {
            var data = GetResult().Select(x => x.Data);
            
            data.ShouldBeNull();
        }

        [Test]
        public void Verify_will_not_throw_when_no_data_present_2()
        {
            var data = GetResultAsync().Select(x => x.Data);
            
            data.ShouldBeNull();
        }

        [Test]
        public void Verify_can_select_empty_list()
        {
            var data = GetResultList(false).Select(x => x.Data);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(0);
        }
    }
}
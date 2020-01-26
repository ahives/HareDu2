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
    public class FilterExtensionsTests :
        HareDuTesting
    {
        [Test]
        public void Verify_can_select_data_list_async_1()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();

            var data = GetResultListAsync(true, id2, id1, id2).Where(x => x.Id == id2);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(2);
            data[0].Id.ShouldBe(id2);
            data[1].Id.ShouldBe(id2);
        }

        [Test]
        public void Verify_can_select_data_list_async_2()
        {
            Guid id = Guid.NewGuid();

            var data = GetResultListAsync(false).Where(x => x.Id == id);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(0);
        }

        [Test]
        public void Verify_can_select_data_list_1()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();

            var data = GetResultList(true, id2, id1, id2).Where(x => x.Id == id2);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(2);
            data[0].Id.ShouldBe(id2);
            data[1].Id.ShouldBe(id2);
        }

        [Test]
        public void Verify_can_select_data_list_2()
        {
            Guid id = Guid.NewGuid();

            var data = GetResultList(false).Where(x => x.Id == id);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(0);
        }
    }
}
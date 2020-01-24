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

        [Test]
        public void Verify_can_select_empty_list()
        {
            var data = GetResultList().Select(x => x.Data);
            
            data.ShouldNotBeNull();
            data.Count.ShouldBe(0);
        }
    }
}
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
    public class LinqExtensionsTests :
        HareDuTesting
    {
        [Test]
        public void Verify_can_find_any_values_async()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();

            bool found = GetResultListAsync(true, id1, id2, id3).Any();
            
            found.ShouldBeTrue();
        }

        [Test]
        public void Verify_can_find_any_values()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();

            bool found = GetResultList(true, id1, id2, id3).Any();
            
            found.ShouldBeTrue();
        }

        [Test]
        public void Verify_can_find_any_values_given_criteria_found_async()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();

            bool found = GetResultListAsync(true, id1, id2, id3).Any(x => x.Id == id2);
            
            found.ShouldBeTrue();
        }

        [Test]
        public void Verify_can_find_any_values_given_criteria_not_found_async()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            Guid id4 = Guid.NewGuid();

            bool found = GetResultListAsync(true, id1, id2, id3).Any(x => x.Id == id4);
            
            found.ShouldBeFalse();
        }

        [Test]
        public void Verify_can_find_any_values_given_criteria_found()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();

            bool found = GetResultList(true, id1, id2, id3).Any(x => x.Id == id2);
            
            found.ShouldBeTrue();
        }

        [Test]
        public void Verify_can_find_any_values_given_criteria_not_found()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            Guid id4 = Guid.NewGuid();

            bool found = GetResultList(true, id1, id2, id3).Any(x => x.Id == id4);
            
            found.ShouldBeFalse();
        }
        
        [Test]
        public void Verify_can_get_first_object_or_default_async()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            
            var result = GetResultListAsync(true, id1, id2, id3).FirstOrDefault();
 
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id1);
        }
        
        [Test]
        public void Verify_can_get_first_object_or_default_predicate_found_async()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            
            var result = GetResultListAsync(true, id1, id2, id3).FirstOrDefault(x => x.Id == id3);
 
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id3);
        }
        
        [Test]
        public void Verify_can_get_first_object_or_default_predicate_not_found_async()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            Guid id4 = Guid.NewGuid();
            
            var result = GetResultListAsync(true, id1, id2, id3).FirstOrDefault(x => x.Id == id4);
 
            result.ShouldBeNull();
        }
        
        [Test]
        public void Verify_can_get_first_object_or_default_1()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            
            var result = GetResultList(true, id1, id2, id3).FirstOrDefault();
 
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id1);
        }
        
        [Test]
        public void Verify_can_get_first_object_or_default_2()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            
            var result = GetResultList(true, id1, id2, id3).FirstOrDefault(x => x.Id == id3);
 
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id3);
        }
        
        [Test]
        public void Verify_can_get_first_object_or_default_3()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            Guid id4 = Guid.NewGuid();
            
            var result = GetResultList(true, id1, id2, id3).FirstOrDefault(x => x.Id == id4);
 
            result.ShouldBeNull();
        }
        
        [Test]
        public void Verify_can_get_first_object_or_default_4()
        {
            var result = GetResultList(false).FirstOrDefault();
 
            result.ShouldBe(default);
        }

        [Test]
        public void Verify_returns_single_object_async()
        {
            Guid id1 = Guid.NewGuid();
            
            var result = GetResultListAsync(true, id1).SingleOrDefault();
 
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id1);
        }

        [Test]
        public void Verify_single_or_default_does_not_throw_async()
        {
            var result = GetResultListAsync(false).SingleOrDefault();
 
            result.ShouldBeNull();
        }

        [Test]
        public void Verify_returns_single_object()
        {
            Guid id = Guid.NewGuid();
            
            var result = GetResultList(true, id).SingleOrDefault();
 
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id);
        }

        [Test]
        public void Verify_does_not_throw_when_list_null()
        {
            var result = GetNullResultList().SingleOrDefault();
 
            result.ShouldBeNull();
        }

        [Test]
        public void Verify_does_not_throw_when_there_are_multiple_elements()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            
            var result = GetResultList(true, id1, id2, id3).SingleOrDefault();
 
            result.ShouldBeNull();
        }

        [Test]
        public void Verify_does_not_throw_when_ResultList_empty_async()
        {
            Guid id1 = Guid.NewGuid();
            
            var result = GetResultListAsync(false).SingleOrDefault(x => x.Id == id1);
 
            result.ShouldBeNull();
        }

        [Test]
        public void Verify_does_not_throw_when_ResultList_null_async()
        {
            Guid id1 = Guid.NewGuid();
            
            var result = GetNullResultListAsync().SingleOrDefault(x => x.Id == id1);
 
            result.ShouldBeNull();
        }

        [Test]
        public void Verify_does_not_throw_when_ResultList_empty()
        {
            Guid id1 = Guid.NewGuid();
            
            var result = GetResultList(false).SingleOrDefault(x => x.Id == id1);
 
            result.ShouldBeNull();
        }

        [Test]
        public void Verify_can_return_single_element()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            
            var result = GetResultList(true, id1, id2, id3).SingleOrDefault(x => x.Id == id2);
 
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id2);
        }

        [Test]
        public void Verify_will_return_default_element()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();
            Guid id4 = Guid.NewGuid();
            
            var result = GetResultList(true, id1, id2, id3).SingleOrDefault(x => x.Id == id4);
 
            result.ShouldBeNull();
        }

        [Test]
        public void Verify_does_not_throw_when_ResultList_has_multiple_results_matching_criteria_async()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            
            var result = GetResultListAsync(true, id1, id2, id2).SingleOrDefault(x => x.Id == id2);
 
            result.ShouldBeNull();
        }

        [Test]
        public void Verify_does_not_throw_when_ResultList_has_multiple_results_matching_criteria()
        {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            
            var result = GetResultListAsync(true, id1, id2, id2).SingleOrDefault(x => x.Id == id2);
 
            result.ShouldBeNull();
        }
    }
}
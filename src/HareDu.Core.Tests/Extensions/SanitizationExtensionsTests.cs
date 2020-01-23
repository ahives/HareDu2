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
    using Core.Extensions;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class SanitizationExtensionsTests
    {
        [Test]
        public void Verify_can_replace_characters()
        {
            string str = "knsfllfdn%5Fnla9949nnkkf%5Fkskfdnknssf";
            
            str.SanitizePropertiesKey().ShouldBe("knsfllfdn%255Fnla9949nnkkf%255Fkskfdnknssf");
        }
        
        [Test]
        public void Verify_does_not_return_null_1()
        {
            string str = "";
            
            str.SanitizePropertiesKey().ShouldBe(string.Empty);
        }
        
        [Test]
        public void Verify_does_not_return_null_2()
        {
            string str = " ";
            
            str.SanitizePropertiesKey().ShouldBe(string.Empty);
        }
        
        [Test]
        public void Verify_does_not_return_null_3()
        {
            string str = null;
            
            str.SanitizePropertiesKey().ShouldBe(string.Empty);
        }

        [Test]
        public void Verify_will_return_vhost_when_nothing_to_sanitize()
        {
            string str = "some_fake_vhost";
            
            str.ToSanitizedName().ShouldBe(str);
        }

        [Test]
        public void Verify_return_empty_when_vhost_is_null_whitespace_or_empty_1()
        {
            string str = "";
            
            str.ToSanitizedName().ShouldBe(string.Empty);
        }

        [Test]
        public void Verify_return_empty_when_vhost_is_null_whitespace_or_empty_2()
        {
            string str = " ";
            
            str.ToSanitizedName().ShouldBe(string.Empty);
        }

        [Test]
        public void Verify_return_empty_when_vhost_is_null_whitespace_or_empty_3()
        {
            string str = null;
            
            str.ToSanitizedName().ShouldBe(string.Empty);
        }

        [Test]
        public void Verify_return_sanitized_vhost_name()
        {
            string str = "/";
            
            str.ToSanitizedName().ShouldBe("%2f");
        }
    }
}
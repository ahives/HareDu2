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
namespace HareDu.Snapshotting.Tests.Extensions
{
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Extensions;

    [TestFixture]
    public class StringFormatExtensionsTests
    {
        [Test]
        public void Verify_returns_bytes()
        {
            ulong bytes = 999;
            
            bytes.ToByteString().ShouldBe("999");
        }
        
        [Test]
        public void Verify_returns_kilobytes()
        {
            ulong bytes = 1001;
            
            bytes.ToByteString().ShouldBe("1.001 KB");
        }
        
        [Test]
        public void Verify_returns_megabytes()
        {
            ulong bytes = 1000000;
            
            bytes.ToByteString().ShouldBe("1.000 MB");
        }
        
        [Test]
        public void Verify_returns_gigabytes()
        {
            ulong bytes = 1000000000;
            
            bytes.ToByteString().ShouldBe("1.000 GB");
        }
    }
}
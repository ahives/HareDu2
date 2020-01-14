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
    using Model;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Extensions;

    [TestFixture]
    public class ConnectionStateExtensionsTests
    {
        [Test]
        public void Verify_can_convert_1()
        {
            ConnectionState state = "starting".Convert();
            
            state.ShouldBe(ConnectionState.Starting);
        }
        
        [Test]
        public void Verify_can_convert_2()
        {
            ConnectionState state = "tuning".Convert();
            
            state.ShouldBe(ConnectionState.Tuning);
        }
        
        [Test]
        public void Verify_can_convert_3()
        {
            ConnectionState state = "opening".Convert();
            
            state.ShouldBe(ConnectionState.Opening);
        }
        
        [Test]
        public void Verify_can_convert_4()
        {
            ConnectionState state = "flow".Convert();
            
            state.ShouldBe(ConnectionState.Flow);
        }
        
        [Test]
        public void Verify_can_convert_5()
        {
            ConnectionState state = "blocking".Convert();
            
            state.ShouldBe(ConnectionState.Blocking);
        }
        
        [Test]
        public void Verify_can_convert_6()
        {
            ConnectionState state = "blocked".Convert();
            
            state.ShouldBe(ConnectionState.Blocked);
        }
        
        [Test]
        public void Verify_can_convert_7()
        {
            ConnectionState state = "closing".Convert();
            
            state.ShouldBe(ConnectionState.Closing);
        }
        
        [Test]
        public void Verify_can_convert_8()
        {
            ConnectionState state = "closed".Convert();
            
            state.ShouldBe(ConnectionState.Closed);
        }
        
        [Test]
        public void Verify_can_convert_9()
        {
            ConnectionState state = "running".Convert();
            
            state.ShouldBe(ConnectionState.Running);
        }
        
        [Test]
        public void Verify_can_convert_10()
        {
            ConnectionState state = "".Convert();
            
            state.ShouldBe(ConnectionState.Inconclusive);
        }
        
        [Test]
        public void Verify_can_convert_11()
        {
            ConnectionState state = "blah".Convert();
            
            state.ShouldBe(ConnectionState.Inconclusive);
        }
    }
}
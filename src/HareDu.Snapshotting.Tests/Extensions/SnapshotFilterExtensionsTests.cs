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
    using System.Collections.Generic;
    using Fakes;
    using HareDu.Model;
    using Model;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Extensions;

    [TestFixture]
    public class SnapshotFilterExtensionsTests
    {
        [Test]
        public void Verify_can_filter_connections_1()
        {
            IReadOnlyList<ChannelInfo> channels = new List<ChannelInfo>
            {
                new FakeChannelInfo(1, 1, 1),
                new FakeChannelInfo(2, 2, 2),
                new FakeChannelInfo(3, 3, 1)
            };

            IReadOnlyList<ChannelSnapshot> snapshots = channels.FilterByConnection("Connection 1");
            
            snapshots.Count.ShouldBe(2);
        }
        
        [Test]
        public void Verify_can_filter_connections_2()
        {
            IReadOnlyList<ChannelInfo> channels = new List<ChannelInfo>
            {
                new FakeChannelInfo(1, 1, 1),
                new FakeChannelInfo(2, 2, 2),
                new FakeChannelInfo(3, 3, 1)
            };

            IReadOnlyList<ChannelSnapshot> snapshots = channels.FilterByConnection("Connection 2");
            
            snapshots.Count.ShouldBe(1);
        }
        
        [Test]
        public void Verify_can_filter_connections_3()
        {
            IReadOnlyList<ChannelInfo> channels = new List<ChannelInfo>
            {
                new FakeChannelInfo(1, 1, 1),
                new FakeChannelInfo(2, 2, 2),
                new FakeChannelInfo(3, 3, 1)
            };

            IReadOnlyList<ChannelSnapshot> snapshots = channels.FilterByConnection("Connection 3");
            
            snapshots.Count.ShouldBe(0);
        }
        
        [Test]
        public void Verify_can_filter_connections_4()
        {
            IReadOnlyList<ChannelInfo> channels = new List<ChannelInfo>();

            IReadOnlyList<ChannelSnapshot> snapshots = channels.FilterByConnection("Connection 1");
            
            snapshots.Count.ShouldBe(0);
        }
        
        [Test]
        public void Verify_can_filter_connections_5()
        {
            IReadOnlyList<ChannelInfo> channels = null;

            IReadOnlyList<ChannelSnapshot> snapshots = channels.FilterByConnection("Connection 1");
            
            snapshots.Count.ShouldBe(0);
        }
    }
}
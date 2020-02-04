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
namespace HareDu.Tests.BrokerObjects
{
    using System.Threading.Tasks;
    using Autofac;
    using NUnit.Framework;
    using Registration;
    using Shouldly;

    [TestFixture]
    public class NodeHealthTests :
        HareDuTesting
    {
        [Test]
        public async Task Verify_can_check_if_named_node_healthy()
        {
            var container = GetContainerBuilder("TestData/NodeHealthInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<NodeHealth>()
                .GetDetails("rabbit@localhost");

            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.Status.ShouldBe("ok");
        }

        [Test]
        public async Task Verify_can_check_if_node_healthy()
        {
            var container = GetContainerBuilder("TestData/NodeHealthInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<NodeHealth>()
                .GetDetails();
            
            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.Status.ShouldBe("ok");
        }
    }
}
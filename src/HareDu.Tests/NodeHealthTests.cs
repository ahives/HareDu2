// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using Autofac;
    using NUnit.Framework;

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

            Assert.IsTrue(result.HasData);
            Assert.IsFalse(result.HasFaulted);
            Assert.AreEqual("ok", result.Data.Status);
        }

        [Test]
        public async Task Verify_can_check_if_node_healthy()
        {
            var container = GetContainerBuilder("TestData/NodeHealthInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<NodeHealth>()
                .GetDetails();
            
            Assert.IsTrue(result.HasData);
            Assert.IsFalse(result.HasFaulted);
            Assert.AreEqual("ok", result.Data.Status);
        }
    }
}
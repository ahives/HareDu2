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
    using Shouldly;

    [TestFixture]
    public class ServerTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_definitions()
        {
            var container = GetContainerBuilder("TestData/ServerDefinitionInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Server>()
                .GetDefinition();
            
            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Bindings.Count.ShouldBe(8);
            result.Data.Exchanges.Count.ShouldBe(11);
            result.Data.Queues.Count.ShouldBe(5);
            result.Data.Parameters.Count.ShouldBe(3);
            result.Data.Permissions.Count.ShouldBe(8);
            result.Data.Policies.Count.ShouldBe(2);
            result.Data.Users.Count.ShouldBe(2);
            result.Data.VirtualHosts.Count.ShouldBe(9);
            result.Data.GlobalParameters.Count.ShouldBe(5);
            result.Data.TopicPermissions.Count.ShouldBe(3);
            result.Data.RabbitMqVersion.ShouldBe("3.7.15");
        }
    }
}
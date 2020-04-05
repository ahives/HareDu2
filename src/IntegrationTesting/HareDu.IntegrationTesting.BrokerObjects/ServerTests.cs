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
namespace HareDu.IntegrationTesting.BrokerObjects
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core;
    using Extensions;
    using Model;
    using NUnit.Framework;
    using Registration;

    [TestFixture]
    public class ServerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDuConfiguration($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .AddHareDu()
                .Build();
        }

        [Test]
        public async Task Should_be_able_to_get_all_definitions()
        {
            Result<ServerInfo> result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Server>()
                .Get()
                .ScreenDump();
        }
    }
}
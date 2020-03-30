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
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using Extensions;
    using NUnit.Framework;
    using Registration;

    [TestFixture]
    public class NodeTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDu()
                .Build();
        }

        [Test]
        public async Task Should_be_able_to_get_all_nodes()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Node>()
                .GetAll()
                .ScreenDump();
        }

        [Test]
        public async Task Verify_can_check_if_named_node_healthy()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Node>()
                .GetHealth("rabbit@localhost");

            if (result.HasData)
            {
                var info = result.Select(x => x.Data);
                
                Console.WriteLine($"Reason: {info.Reason}");
                Console.WriteLine($"Status: {info.Status}");
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
//            Console.WriteLine(result.DebugInfo.URL);
        }

        [Test]
        public async Task Verify_can_check_if_node_healthy()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Node>()
                .GetHealth();
            
            Console.WriteLine((string) result.DebugInfo.URL);
        }
    }
}
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
    using Core;
    using Core.Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class NodeHealthTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuModule>();

            _container = builder.Build();
        }

        [Test]
        public async Task Verify_can_check_if_named_node_healthy()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<NodeHealth>()
                .GetDetails("rabbit@localhost");

            if (result.HasData)
            {
                var info = result.Select(x => x.Data);
                
                Console.WriteLine((string) "Reason: {0}", (object) info.Reason);
                Console.WriteLine((string) "Status: {0}", (object) info.Status);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
//            Console.WriteLine(result.DebugInfo.URL);
        }

        [Test]
        public async Task Verify_can_check_if_node_healthy()
        {
            Result<NodeHealthInfo> result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<NodeHealth>()
                .GetDetails();
            
            Console.WriteLine((string) result.DebugInfo.URL);
        }
    }
}
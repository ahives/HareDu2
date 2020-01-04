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
    using NUnit.Framework;

    [TestFixture]
    public class NodeTests
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
        public async Task Should_be_able_to_get_all_nodes()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Node>()
                .GetAll();
            
            foreach (var node in result.Select(x => x.Data))
            {
                Console.WriteLine((string) "OperatingSystemPid: {0}", (object) node.OperatingSystemProcessId);
                Console.WriteLine((string) "TotalFileDescriptors: {0}", (object) node.TotalFileDescriptors);
                Console.WriteLine((string) "MemoryUsedDetailsRate: {0}", (object) node.MemoryUsageDetails.Rate);
                Console.WriteLine((string) "FileDescriptorUsedDetailsRate: {0}", (object) node.FileDescriptorUsedDetails.Rate);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
    }
}
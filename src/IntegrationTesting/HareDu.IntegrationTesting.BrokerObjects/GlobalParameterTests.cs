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
    public class GlobalParameterTests
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
        public async Task Should_be_able_to_get_all_global_parameters()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .GetAll();

            foreach (var parameter in result.Select(x => x.Data))
            {
                Console.WriteLine((string) "Name: {0}", (object) parameter.Name);
                Console.WriteLine((string) "Value: {0}", (object) parameter.Value);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
        
        [Test]
        public async Task Verify_can_create_parameter()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter("fake_param2");
                    x.Argument("fake_value");
//                    x.Arguments(arg =>
//                    {
//                        arg.Set("arg1", "value1");
//                        arg.Set("arg2", "value2");
//                    });
                });
             
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine((string) result.ToJsonString());
        }
        
        [Test]
        public async Task Verify_can_delete_parameter()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Delete(x => x.Parameter("Fred"));
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine((string) result.ToJsonString());
        }
    }
}
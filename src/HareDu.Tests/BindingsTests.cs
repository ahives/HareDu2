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
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class BindingsTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuModule>();

            _container = builder.Build();
        }

        [Test, Explicit]
        public async Task Should_be_able_to_get_all_bindings()
        {
            var result = await _container.Resolve<IRmqObjectFactory>()
                .Object<Binding>()
                .GetAll();
            
            foreach (var binding in result.Select(x => x.Data))
            {
                Console.WriteLine("VirtualHost: {0}", binding.VirtualHost);
                Console.WriteLine("Source: {0}", binding.Source);
                Console.WriteLine("Destination: {0}", binding.Destination);
                Console.WriteLine("DestinationType: {0}", binding.DestinationType);
                Console.WriteLine("RoutingKey: {0}", binding.RoutingKey);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_add_arguments()
        {
            var result = await _container.Resolve<IRmqObjectFactory>()
                .Object<Binding>()
                .Create(x =>
                {
                    x.Binding(b =>
                    {
                        b.Source("E2");
                        b.Destination("Q1");
                        b.Type(BindingType.Exchange);
                    });
                    x.Configure(c =>
                    {
                        c.HasRoutingKey("*.");
                        c.HasArguments(arg =>
                        {
                            arg.Set("arg1", "value1");
                        });
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_delete_binding()
        {
            var result = await _container.Resolve<IRmqObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name("%2A.");
                        b.Source("E2");
                        b.Destination("Q4");
                        b.Type(BindingType.Queue);
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }
    }
}
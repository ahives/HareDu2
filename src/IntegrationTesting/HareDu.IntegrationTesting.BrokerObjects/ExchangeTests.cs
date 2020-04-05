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
    using Core.Configuration;
    using Core.Extensions;
    using CoreIntegration;
    using Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Registration;
    using Shouldly;

    [TestFixture]
    public class ExchangeTests
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
        public async Task Should_be_able_to_get_all_exchanges()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Exchange>()
                .GetAll()
                .ScreenDump();

            result.HasFaulted.ShouldBeFalse();
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Should_be_able_to_get_all_exchanges_2()
        {
            var services = new ServiceCollection()
                .AddHareDuConfiguration($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .AddHareDu()
                .BuildServiceProvider();
            
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Exchange>()
                .GetAll();

            foreach (var exchange in result.Select(x => x.Data))
            {
                Console.WriteLine("Name: {0}", exchange.Name);
                Console.WriteLine("VirtualHost: {0}", exchange.VirtualHost);
                Console.WriteLine("AutoDelete: {0}", exchange.AutoDelete);
                Console.WriteLine("Internal: {0}", exchange.Internal);
                Console.WriteLine("Durable: {0}", exchange.Durable);
                Console.WriteLine("RoutingType: {0}", exchange.RoutingType);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
            
            result.HasFaulted.ShouldBeFalse();
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Should_be_able_to_get_all_exchanges_3()
        {
            // var provider = new BrokerConfigProvider();
            var provider = new HareDuConfigProvider();
            var config = provider.Configure(x => { });
            var factory = new BrokerObjectFactory(config);
            
            var result = await factory
                .Object<Exchange>()
                .GetAll()
                .ScreenDump();
            
            result.HasFaulted.ShouldBeFalse();
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_filter_exchanges()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Exchange>()
                .GetAll();

            result
                .Where(x => x.Name == "amq.fanout")
                .ScreenDump();
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_create_exchange()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Exchange>()
                .Create(x =>
                {
                    x.Exchange("E5");
                    x.Configure(c =>
                    {
                        c.IsDurable();
                        c.IsForInternalUse();
                        c.HasRoutingType(ExchangeRoutingType.Fanout);
//                        c.HasArguments(arg =>
//                        {
//                            arg.Set("", "");
//                        });
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_delete_exchange()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Exchange>()
                .Delete(x =>
                {
                    x.Exchange("E3");
                    x.Targeting(t => t.VirtualHost("HareDu"));
                    x.WithConditions(c => c.IfUnused());
                });
            
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }
    }
}
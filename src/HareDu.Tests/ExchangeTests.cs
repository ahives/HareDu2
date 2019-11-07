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
    using Core.Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ExchangeTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_exchanges()
        {
            var container = GetContainerBuilder("TestData/ExchangeInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Exchange>()
                .GetAll();
            
            Assert.IsFalse(result.HasFaulted);
            Assert.IsTrue(result.HasData);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(9, result.Data.Count);
            Assert.IsTrue(result.Data[1].Durable);
            Assert.IsFalse(result.Data[1].Internal);
            Assert.IsFalse(result.Data[1].AutoDelete);
            Assert.AreEqual("E2", result.Data[1].Name);
            Assert.AreEqual("direct", result.Data[1].RoutingType);
            Assert.AreEqual("HareDu", result.Data[1].VirtualHost);
            Assert.IsNotNull(result.Data[1].Arguments);
            Assert.AreEqual(1, result.Data[1].Arguments.Count);
            Assert.AreEqual("exchange", result.Data[1].Arguments["alternate-exchange"]);
        }

        [Test]
        public async Task Verify_can_create_exchange()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Exchange>()
                .Create(x =>
                {
                    x.Exchange("fake_exchange");
                    x.Configure(c =>
                    {
                        c.IsDurable();
                        c.IsForInternalUse();
                        c.HasRoutingType(ExchangeRoutingType.Fanout);
                        c.HasArguments(arg =>
                        {
                            arg.Set("fake_arg", "8238b");
                        });
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            
            ExchangeDefinition definition = result.DebugInfo.Request.ToObject<ExchangeDefinition>();

            Assert.AreEqual("api/exchanges/HareDu/fake_exchange", result.DebugInfo.URL);
            Assert.AreEqual("fanout", definition.RoutingType);
            Assert.IsTrue(definition.Durable);
            Assert.IsTrue(definition.Internal);
            Assert.IsFalse(definition.AutoDelete);
            Assert.AreEqual(1, definition.Arguments.Count);
            Assert.AreEqual("8238b", definition.Arguments["fake_arg"]);
        }

        [Test]
        public async Task Verify_can_delete_exchange()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Exchange>()
                .Delete(x =>
                {
                    x.Exchange("E3");
                    x.Target(t => t.VirtualHost("HareDu"));
                    x.WithConditions(c => c.IfUnused());
                });
            
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }
    }
}
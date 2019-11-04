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
    using Core.Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class BindingsTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_bindings()
        {
            var container = GetContainerBuilder("TestData/BindingInfo1.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .GetAll();
            
            Assert.IsTrue(result.HasData);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(12, result.Data.Count);
            Assert.IsFalse(result.HasFaulted);
        }

        [Test]
        public async Task Verify_can_add_arguments()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
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

            BindingDefinition definition = result.DebugInfo.Request.ToObject<BindingDefinition>();
            
            Assert.AreEqual("*.", definition.RoutingKey);
            Assert.AreEqual("value1", definition.Arguments["arg1"]);
            Assert.IsFalse(result.HasFaulted);
        }

        [Test]
        public async Task Verify_can_delete_binding()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name("Binding1");
                        b.Source("E2");
                        b.Destination("Q4");
                        b.Type(BindingType.Queue);
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Assert.AreEqual("api/bindings/HareDu/e/E2/q/Q4/Binding1", result.DebugInfo.URL);
        }
    }
}
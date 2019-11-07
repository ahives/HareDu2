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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Core.Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class GlobalParameterTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_global_parameters()
        {
            var container = GetContainerBuilder("TestData/GlobalParameterInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .GetAll();

            Assert.IsTrue(result.HasData);
            Assert.AreEqual(2, result.Data.Count);
            Assert.IsFalse(result.HasFaulted);
            Assert.AreEqual("cluster_name", result.Data[0].Name);
            Assert.AreEqual("rabbit@haredu", result.Data[0].Value);
        }
        
        [Test]
        public async Task Verify_can_create_parameter1()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter("fake_param");
                    x.Argument("fake_value");
                });
             
            Assert.IsFalse(result.HasFaulted);
            
            GlobalParameterDefinition definition = result.DebugInfo.Request.ToObject<GlobalParameterDefinition>();
            
            Assert.AreEqual("fake_param", definition.Name);
            Assert.AreEqual("fake_value", definition.Value);
        }
        
        [Test]
        public async Task Verify_can_create_parameter2()
        {
            var container = GetContainerBuilder("TestData/ExchangeInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter("fake_param");
                    x.Arguments(arg =>
                    {
                        arg.Set("arg1", "value1");
                        arg.Set("arg2", 5);
                    });
                });
             
            Assert.IsFalse(result.HasFaulted);
            
            GlobalParameterDefinition definition = result.DebugInfo.Request.ToObject<GlobalParameterDefinition>();
            
            Assert.AreEqual("fake_param", definition.Name);

//            var value = definition.Value.Cast<Dictionary<string, object>>();
//            Console.WriteLine(value);
//            Assert.IsNotEmpty(value);
//            Assert.AreEqual("value1", value["arg1"]);
//            Assert.AreEqual(5, value["arg2"]);
        }
        
        [Test]
        public async Task Verify_can_delete_parameter()
        {
            var container = GetContainerBuilder("TestData/ExchangeInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Delete(x => x.Parameter("Fred"));
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine((string) result.ToJsonString());
        }
    }
}
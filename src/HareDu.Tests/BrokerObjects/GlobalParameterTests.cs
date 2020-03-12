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
namespace HareDu.Tests.BrokerObjects
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Core.Extensions;
    using HareDu.Registration;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class GlobalParameterTests :
        HareDuTesting
    {
        [Test]
        public void Should_be_able_to_get_all_global_parameters()
        {
            var container = GetContainerBuilder("TestData/GlobalParameterInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .GetAll()
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.Count.ShouldBe(5);
            result.Data[3].Name.ShouldBe("fake_param1");
            
            var value = result.Data[3].Value.ToString().ToObject<IDictionary<string, object>>();
            
            value.Count.ShouldBe(2);
            value["arg1"].ShouldBe("value1");
            value["arg2"].ShouldBe("value2");
        }
        
        [Test]
        public void Verify_can_create_parameter_1()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter("fake_param");
                    x.Value("fake_value");
                })
                .GetResult();
             
            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();
            
            GlobalParameterDefinition definition = result.DebugInfo.Request.ToObject<GlobalParameterDefinition>();
            
            definition.Name.ShouldBe("fake_param");
            definition.Value.ShouldBe("fake_value");
        }
        
        [Test]
        public void Verify_can_create_parameter_2()
        {
            var container = GetContainerBuilder("TestData/ExchangeInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter("fake_param");
                    x.Value(arg =>
                    {
                        arg.Set("arg1", "value1");
                        arg.Set("arg2", 5);
                    });
                })
                .GetResult();
             
            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();
            
            GlobalParameterDefinition definition = result.DebugInfo.Request.ToObject<GlobalParameterDefinition>();
            
            definition.Name.ShouldBe("fake_param");
            definition.Value
                .ToString()
                .ToObject<IDictionary<string, object>>()["arg1"]
                .Cast<string>()
                .ShouldBe("value1");
            definition.Value
                .ToString()
                .ToObject<IDictionary<string, object>>()["arg2"]
                .ShouldBe(5);
        }
        
        [Test]
        public void Verify_cannot_create_parameter_1()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter(string.Empty);
                    x.Value("fake_value");
                })
                .GetResult();
             
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            GlobalParameterDefinition definition = result.DebugInfo.Request.ToObject<GlobalParameterDefinition>();
            
            definition.Name.ShouldBeNullOrEmpty();
            definition.Value.ShouldBe("fake_value");
        }
        
        [Test]
        public void Verify_cannot_create_parameter_2()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Value("fake_value");
                })
                .GetResult();
             
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            GlobalParameterDefinition definition = result.DebugInfo.Request.ToObject<GlobalParameterDefinition>();
            
            definition.Name.ShouldBeNullOrEmpty();
            definition.Value.ShouldBe("fake_value");
        }
        
        [Test]
        public void Verify_cannot_create_parameter_3()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter("fake_param");
                    x.Value(string.Empty);
                })
                .GetResult();
             
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            GlobalParameterDefinition definition = result.DebugInfo.Request.ToObject<GlobalParameterDefinition>();
            
            definition.Name.ShouldBe("fake_param");
            definition.Value.Cast<string>().ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_parameter_4()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter(string.Empty);
                    x.Value(string.Empty);
                })
                .GetResult();
             
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            GlobalParameterDefinition definition = result.DebugInfo.Request.ToObject<GlobalParameterDefinition>();
            
            definition.Name.ShouldBeNullOrEmpty();
            definition.Value.Cast<string>().ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_parameter_5()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter(string.Empty);
                })
                .GetResult();
             
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            GlobalParameterDefinition definition = result.DebugInfo.Request.ToObject<GlobalParameterDefinition>();
            
            definition.Name.ShouldBeNullOrEmpty();
            definition.Value.ShouldBeNull();
        }
        
        [Test]
        public void Verify_cannot_create_parameter_6()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Value(string.Empty);
                })
                .GetResult();
             
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            GlobalParameterDefinition definition = result.DebugInfo.Request.ToObject<GlobalParameterDefinition>();
            
            definition.Name.ShouldBeNullOrEmpty();
            definition.Value.Cast<string>().ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_parameter_7()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                })
                .GetResult();
             
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            GlobalParameterDefinition definition = result.DebugInfo.Request.ToObject<GlobalParameterDefinition>();
            
            definition.Name.ShouldBeNullOrEmpty();
            definition.Value.ShouldBeNull();
        }
        
        [Test]
        public void Verify_can_delete_parameter()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Delete(x => x.Parameter("fake_param"))
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
        }
        
        [Test]
        public void Verify_cannot_delete_parameter_1()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Delete(x => x.Parameter(string.Empty))
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }
        
        [Test]
        public void Verify_cannot_delete_parameter_2()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Delete(x => {})
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }
    }
}
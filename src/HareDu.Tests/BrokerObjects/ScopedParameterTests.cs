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
    using Autofac;
    using Core.Extensions;
    using HareDu.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ScopedParameterTests :
        HareDuTesting
    {
        [Test]
        public void Should_be_able_to_get_all_scoped_parameters()
        {
            var container = GetContainerBuilder("TestData/ScopedParameterInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .GetAll()
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBe(3);
            result.Data[0].ShouldNotBeNull();
            result.Data[0].Value.Count.ShouldBe(2);
            result.Data[0].Value["max-connections"].Cast<long>().ShouldBe(10);
            result.Data[0].Value["max-queues"].Cast<string>().ShouldBe("value");
        }
        
        [Test]
        public void Verify_can_create_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<long>(x =>
                {
                    x.Parameter("fake_parameter", 89);
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<long> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<long>>();
            
            definition.Component.ShouldBe("fake_component");
            definition.ParameterName.ShouldBe("fake_parameter");
            definition.ParameterValue.ShouldBe(89);
            definition.VirtualHost.ShouldBe("HareDu");
        }
        
        [Test]
        public void Verify_can_create_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Parameter("fake_parameter", "value");
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBe("fake_component");
            definition.ParameterName.ShouldBe("fake_parameter");
            definition.ParameterValue.ShouldBe("value");
            definition.VirtualHost.ShouldBe("HareDu");
        }
        
        [Test]
        public void Verify_cannot_create_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Parameter(string.Empty, "value");
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBe("fake_component");
            definition.ParameterName.ShouldBeNullOrEmpty();
            definition.ParameterValue.ShouldBe("value");
            definition.VirtualHost.ShouldBe("HareDu");
        }
        
        [Test]
        public void Verify_cannot_create_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBe("fake_component");
            definition.ParameterName.ShouldBeNullOrEmpty();
            definition.ParameterValue.ShouldBeNullOrEmpty();
            definition.VirtualHost.ShouldBe("HareDu");
        }
        
        [Test]
        public void Verify_cannot_create_3()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Parameter("fake_parameter", "value");
                    x.Targeting(t =>
                    {
                        t.Component(string.Empty);
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBeNullOrEmpty();
            definition.ParameterName.ShouldBe("fake_parameter");
            definition.ParameterValue.ShouldBe("value");
            definition.VirtualHost.ShouldBe("HareDu");
        }
        
        [Test]
        public void Verify_cannot_create_4()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Parameter("fake_parameter", "value");
                    x.Targeting(t =>
                    {
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBeNullOrEmpty();
            definition.ParameterName.ShouldBe("fake_parameter");
            definition.ParameterValue.ShouldBe("value");
            definition.VirtualHost.ShouldBe("HareDu");
        }
        
        [Test]
        public void Verify_cannot_create_5()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Parameter("fake_parameter", "value");
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                        t.VirtualHost(string.Empty);
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBe("fake_component");
            definition.ParameterName.ShouldBe("fake_parameter");
            definition.ParameterValue.ShouldBe("value");
            definition.VirtualHost.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_6()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Parameter("fake_parameter", "value");
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBe("fake_component");
            definition.ParameterName.ShouldBe("fake_parameter");
            definition.ParameterValue.ShouldBe("value");
            definition.VirtualHost.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_7()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Parameter("fake_parameter", "value");
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                        t.VirtualHost(string.Empty);
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBe("fake_component");
            definition.ParameterName.ShouldBe("fake_parameter");
            definition.ParameterValue.ShouldBe("value");
            definition.VirtualHost.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_8()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Parameter(string.Empty, "value");
                    x.Targeting(t =>
                    {
                        t.Component(string.Empty);
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBeNullOrEmpty();
            definition.ParameterName.ShouldBeNullOrEmpty();
            definition.ParameterValue.ShouldBe("value");
            definition.VirtualHost.ShouldBe("HareDu");
        }
        
        [Test]
        public void Verify_cannot_create_9()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Targeting(t =>
                    {
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBeNullOrEmpty();
            definition.ParameterName.ShouldBeNullOrEmpty();
            definition.ParameterValue.ShouldBeNullOrEmpty();
            definition.VirtualHost.ShouldBe("HareDu");
        }
        
        [Test]
        public void Verify_cannot_create_10()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Parameter("fake_parameter", "value");
                    x.Targeting(t =>
                    {
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBeNullOrEmpty();
            definition.ParameterName.ShouldBe("fake_parameter");
            definition.ParameterValue.ShouldBe("value");
            definition.VirtualHost.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_11()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Parameter("fake_parameter", "value");
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBeNullOrEmpty();
            definition.ParameterName.ShouldBe("fake_parameter");
            definition.ParameterValue.ShouldBe("value");
            definition.VirtualHost.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_12()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
            result.DebugInfo.ShouldNotBeNull();
            
            ScopedParameterDefinition<string> definition = result.DebugInfo.Request.ToObject<ScopedParameterDefinition<string>>();
            
            definition.Component.ShouldBeNullOrEmpty();
            definition.ParameterName.ShouldBeNullOrEmpty();
            definition.ParameterValue.ShouldBeNullOrEmpty();
            definition.VirtualHost.ShouldBeNullOrEmpty();
        }

        [Test]
        public void Verify_can_delete()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter("fake_parameter");
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_cannot_delete_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter(string.Empty);
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_3()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter("fake_parameter");
                    x.Targeting(t =>
                    {
                        t.Component(string.Empty);
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_4()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter("fake_parameter");
                    x.Targeting(t =>
                    {
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_5()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter("fake_parameter");
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                        t.VirtualHost(string.Empty);
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_6()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter("fake_parameter");
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_7()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter(string.Empty);
                    x.Targeting(t =>
                    {
                        t.Component(string.Empty);
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_delete_8()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Targeting(t =>
                    {
                        t.VirtualHost("HareDu");
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_delete_9()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter(string.Empty);
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                        t.VirtualHost(string.Empty);
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_delete_10()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Targeting(t =>
                    {
                        t.Component("fake_component");
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_delete_11()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter(string.Empty);
                    x.Targeting(t =>
                    {
                        t.Component(string.Empty);
                        t.VirtualHost(string.Empty);
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
        }

        [Test]
        public void Verify_cannot_delete_12()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Targeting(t =>
                    {
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
        }

        [Test]
        public void Verify_cannot_delete_13()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter(string.Empty);
                    x.Targeting(t =>
                    {
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
        }

        [Test]
        public void Verify_cannot_delete_14()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter(string.Empty);
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
        }

        [Test]
        public void Verify_cannot_delete_15()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
        }
    }
}
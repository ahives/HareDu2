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
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class BindingsTests :
        HareDuTesting
    {
        [Test]
        public void Should_be_able_to_get_all_bindings()
        {
            var container = GetContainerBuilder("TestData/BindingInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .GetAll()
                .GetResult();
            
            result.HasData.ShouldBeTrue();
            result.Data.Count.ShouldBe(12);
            result.HasFaulted.ShouldBeFalse();
            result.Data.ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_add_arguments()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
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
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();

            BindingDefinition definition = result.DebugInfo.Request.ToObject<BindingDefinition>();
            
            definition.RoutingKey.ShouldBe("*.");
            definition.Arguments["arg1"].ShouldBe("value1");
        }

        [Test]
        public void Verify_cannot_add_arguments_1()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create(x =>
                {
                    x.Binding(b =>
                    {
                        b.Source(string.Empty);
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
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_add_arguments_2()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create(x =>
                {
                    x.Binding(b =>
                    {
                        b.Source("E2");
                        b.Destination(string.Empty);
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
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_add_arguments_3()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create(x =>
                {
                    x.Binding(b =>
                    {
                        b.Source(string.Empty);
                        b.Destination(string.Empty);
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
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_add_arguments_4()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
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
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_add_arguments_5()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create(x =>
                {
                    x.Binding(b =>
                    {
                        b.Source(string.Empty);
                        b.Destination(string.Empty);
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
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
        }

        [Test]
        public void Verify_cannot_add_arguments_6()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create(x =>
                {
                    x.Binding(b =>
                    {
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
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_add_arguments_7()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create(x =>
                {
                    x.Binding(b =>
                    {
                        b.Source("E2");
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
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_add_arguments_8()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create(x =>
                {
                    x.Binding(b =>
                    {
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
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_add_arguments_9()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_add_arguments_10()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create(x =>
                {
                    x.Binding(b =>
                    {
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
        }

//        [Test]
//        public async Task Verify_cannot_add_arguments_()
//        {
//            var container = GetContainerBuilder().Build();
//            var result = await container.Resolve<IBrokerObjectFactory>()
//                .Object<Binding>()
//                .Create(x =>
//                {
//                    x.Binding(b =>
//                    {
//                        b.Source("E2");
//                        b.Destination("Q1");
//                        b.Type(BindingType.Exchange);
//                    });
//                    x.Configure(c =>
//                    {
//                        c.HasRoutingKey("*.");
//                        c.HasArguments(arg =>
//                        {
//                            arg.Set("arg1", "value1");
//                        });
//                    });
//                    x.Target(t => t.VirtualHost("HareDu"));
//                });
//
//            result.HasFaulted.ShouldBeTrue();
//            result.Errors.Count.ShouldBe(1);
//        }

        [Test]
        public void Verify_can_delete_binding()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
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
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();
            result.DebugInfo.URL.ShouldBe("api/bindings/HareDu/e/E2/q/Q4/Binding1");
        }

        [Test]
        public void Verify_cannot_delete_binding_1()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name(string.Empty);
                        b.Source("E2");
                        b.Destination("Q4");
                        b.Type(BindingType.Queue);
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_binding_2()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name("Binding1");
                        b.Source("E2");
                        b.Destination(string.Empty);
                        b.Type(BindingType.Queue);
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_binding_3()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name(string.Empty);
                        b.Source("E2");
                        b.Destination(string.Empty);
                        b.Type(BindingType.Queue);
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_delete_binding_4()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name(string.Empty);
                        b.Source("E2");
                        b.Destination(string.Empty);
                        b.Type(BindingType.Queue);
                    });
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
        }

        [Test]
        public void Verify_cannot_delete_binding_5()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name(string.Empty);
                        b.Source("E2");
                        b.Destination("Q4");
                        b.Type(BindingType.Queue);
                    });
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_delete_binding_6()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name("Binding1");
                        b.Source("E2");
                        b.Destination(string.Empty);
                        b.Type(BindingType.Queue);
                    });
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_delete_binding_7()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Source("E2");
                        b.Destination("Q4");
                        b.Type(BindingType.Queue);
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_binding_8()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_delete_binding_9()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
        }

//        [Test]
//        public async Task Verify_cannot_delete_binding_()
//        {
//            var container = GetContainerBuilder().Build();
//            var result = await container.Resolve<IBrokerObjectFactory>()
//                .Object<Binding>()
//                .Delete(x =>
//                {
//                    x.Binding(b =>
//                    {
//                        b.Name("Binding1");
//                        b.Source("E2");
//                        b.Destination("Q4");
//                        b.Type(BindingType.Queue);
//                    });
//                    x.Target(t => t.VirtualHost("HareDu"));
//                });
//            
//            result.HasFaulted.ShouldBeTrue();
//            result.Errors.Count.ShouldBe(1);
//        }
    }
}
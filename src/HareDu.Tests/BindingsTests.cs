namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using Core.Extensions;
    using HareDu.Extensions;
    using Internal.Model;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Serialization;
    using Shouldly;

    [TestFixture]
    public class BindingsTests :
        HareDuTesting
    {
        [Test]
        public void Should_be_able_to_get_all_bindings()
        {
            var container = GetContainerBuilder("TestData/BindingInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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

            BindingDefinition definition = result.DebugInfo.Request.ToObject<BindingDefinition>(Deserializer.Options);
            
            definition.RoutingKey.ShouldBe("*.");
            definition.Arguments["arg1"].ToString().ShouldBe("value1");
        }

        [Test]
        public void Verify_cannot_add_arguments_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
        

        [Test]
        public async Task Verify_able_to_get_all_bindings2()
        {
            var services = GetContainerBuilder("TestData/BindingInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetAllBindings();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasData);
                Assert.AreEqual(12, result.Data.Count);
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.Data);
            });
        }

        [Test]
        public async Task Verify_can_create_exchange_binding_without_arguments1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create("E2", "Q1", BindingType.Exchange, "HareDu");

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.IsNotNull(result.DebugInfo.Request);

                BindingRequest request = result.DebugInfo.Request.ToObject<BindingRequest>(Deserializer.Options);
                
                Assert.That(request.BindingKey, Is.Empty.Or.Null);
                Assert.IsNull(request.Arguments);
            });
        }

        [Test]
        public async Task Verify_can_create_exchange_binding_without_arguments2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBinding("E2", "Q1", "HareDu");

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.IsNotNull(result.DebugInfo.Request);

                BindingRequest request = result.DebugInfo.Request.ToObject<BindingRequest>(Deserializer.Options);
                
                Assert.That(request.BindingKey, Is.Empty.Or.Null);
                Assert.IsNull(request.Arguments);
            });
        }

        [Test]
        public async Task Verify_can_create_queue_binding_without_arguments1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create("E2", "Q1", BindingType.Queue, "HareDu");

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.IsNotNull(result.DebugInfo.Request);

                BindingRequest request = result.DebugInfo.Request.ToObject<BindingRequest>(Deserializer.Options);
                
                Assert.That(request.BindingKey, Is.Empty.Or.Null);
                Assert.IsNull(request.Arguments);
            });
        }

        [Test]
        public async Task Verify_can_create_queue_binding_without_arguments2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBindingToQueue("E2", "Q1", "HareDu");

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.IsNotNull(result.DebugInfo.Request);

                BindingRequest request = result.DebugInfo.Request.ToObject<BindingRequest>(Deserializer.Options);
                
                Assert.That(request.BindingKey, Is.Empty.Or.Null);
                Assert.IsNull(request.Arguments);
            });
        }

        [Test]
        public async Task Verify_can_create_exchange_binding_with_arguments1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create("E2", "Q1", BindingType.Exchange, "HareDu", "*.", x =>
                {
                    x.Add("arg1", "value1");
                });

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.IsNotNull(result.DebugInfo.Request);
                
                BindingRequest request = result.DebugInfo.Request.ToObject<BindingRequest>(Deserializer.Options);

                Assert.AreEqual("*.", request.BindingKey);
                Assert.AreEqual("value1", request.Arguments["arg1"].ToString());
            });
        }

        [Test]
        public async Task Verify_can_create_exchange_binding_with_arguments2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBinding("E2", "Q1", "HareDu", "*.", x =>
                {
                    x.Add("arg1", "value1");
                });

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.IsNotNull(result.DebugInfo.Request);
                
                BindingRequest request = result.DebugInfo.Request.ToObject<BindingRequest>(Deserializer.Options);

                Assert.AreEqual("*.", request.BindingKey);
                Assert.AreEqual("value1", request.Arguments["arg1"].ToString());
            });
        }

        [Test]
        public async Task Verify_can_create_queue_binding_with_arguments1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create("E2", "Q1", BindingType.Queue, "HareDu", "*.", x =>
                {
                    x.Add("arg1", "value1");
                });

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.IsNotNull(result.DebugInfo.Request);
                
                BindingRequest request = result.DebugInfo.Request.ToObject<BindingRequest>(Deserializer.Options);

                Assert.AreEqual("*.", request.BindingKey);
                Assert.AreEqual("value1", request.Arguments["arg1"].ToString());
            });
        }

        [Test]
        public async Task Verify_can_create_queue_binding_with_arguments2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBindingToQueue("E2", "Q1", "HareDu", "*.", x =>
                {
                    x.Add("arg1", "value1");
                });

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.IsNotNull(result.DebugInfo.Request);
                
                BindingRequest request = result.DebugInfo.Request.ToObject<BindingRequest>(Deserializer.Options);

                Assert.AreEqual("*.", request.BindingKey);
                Assert.AreEqual("value1", request.Arguments["arg1"].ToString());
            });
        }

        [Test]
        public async Task Verify_cannot_create_exchange_binding_without_arguments1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create(string.Empty, "Q1", BindingType.Exchange, "HareDu");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_create_exchange_binding_without_arguments2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create("E1", string.Empty, BindingType.Exchange, "HareDu");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_create_exchange_binding_without_arguments3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBinding(string.Empty, "Q1", "HareDu");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_create_exchange_binding_without_arguments4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBinding("E1", string.Empty, "HareDu");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_create_queue_binding_without_arguments1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBindingToQueue(string.Empty, "Q1", "HareDu");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_create_queue_binding_without_arguments2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBindingToQueue("E1", string.Empty, "HareDu");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_create_queue_binding_without_arguments3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBindingToQueue(string.Empty, string.Empty, "HareDu");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_create_queue_binding_without_arguments4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBindingToQueue("E1", "Q1", string.Empty);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_can_delete_queue_binding1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete("E1", "Q1", string.Empty, "HareDu", BindingType.Queue);
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.AreEqual(0, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_can_delete_queue_binding2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBindingToQueue("E1", "Q1", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.AreEqual(0, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_can_delete_queue_binding3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete("E1", "Q1", string.Empty, "HareDu", BindingType.Exchange);
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.AreEqual(0, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_can_delete_queue_binding4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateExchangeBinding("E1", "Q1", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.AreEqual(0, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_queue_binding1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete("E2", string.Empty, string.Empty, "HareDu", BindingType.Queue);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_queue_binding2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteQueueBinding("E2", string.Empty, string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_queue_binding3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(string.Empty, string.Empty, string.Empty, "HareDu", BindingType.Queue);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_queue_binding4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteQueueBinding(string.Empty, string.Empty, string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_queue_binding5()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(string.Empty, string.Empty, string.Empty, string.Empty, BindingType.Queue);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(3, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_queue_binding6()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteQueueBinding(string.Empty, string.Empty, string.Empty, string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(3, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_exchange_binding1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete("E2", string.Empty, string.Empty, "HareDu", BindingType.Exchange);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_exchange_binding2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteExchangeBinding("E2", string.Empty, string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_exchange_binding3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(string.Empty, string.Empty, string.Empty, "HareDu", BindingType.Exchange);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_exchange_binding4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteExchangeBinding(string.Empty, string.Empty, string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_exchange_binding5()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(string.Empty, string.Empty, string.Empty, string.Empty, BindingType.Exchange);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(3, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_exchange_binding6()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteExchangeBinding(string.Empty, string.Empty, string.Empty, string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(3, result.DebugInfo.Errors.Count);
            });
        }
    }
}
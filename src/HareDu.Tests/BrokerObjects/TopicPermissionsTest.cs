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
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class TopicPermissionsTest :
        HareDuTesting
    {
        [Test]
        public void Verify_can_get_all_topic_permissions()
        {
            var container = GetContainerBuilder("TestData/TopicPermissionsInfo.json").Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .GetAll()
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_can_create_user_permissions()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu");
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_cannot_create_topic_permissions_1()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.User(string.Empty);
                    x.VirtualHost("HareDu");
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            TopicPermissionsDefinition definition = result.DebugInfo.Request.ToObject<TopicPermissionsDefinition>();
            
            definition.Exchange.ShouldBe("E4");
            definition.Read.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_topic_permissions_2()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.VirtualHost("HareDu");
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            TopicPermissionsDefinition definition = result.DebugInfo.Request.ToObject<TopicPermissionsDefinition>();
            
            definition.Exchange.ShouldBe("E4");
            definition.Read.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_topic_permissions_3()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.User("guest");
                    x.VirtualHost(string.Empty);
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            TopicPermissionsDefinition definition = result.DebugInfo.Request.ToObject<TopicPermissionsDefinition>();
            
            definition.Exchange.ShouldBe("E4");
            definition.Read.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_topic_permissions_4()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.User("guest");
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            TopicPermissionsDefinition definition = result.DebugInfo.Request.ToObject<TopicPermissionsDefinition>();
            
            definition.Exchange.ShouldBe("E4");
            definition.Read.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_topic_permissions_5()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu");
                    x.Configure(c =>
                    {
                        c.OnExchange(string.Empty);
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            TopicPermissionsDefinition definition = result.DebugInfo.Request.ToObject<TopicPermissionsDefinition>();
            
            definition.Exchange.ShouldBeNullOrEmpty();
            definition.Read.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_topic_permissions_6()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu");
                    x.Configure(c =>
                    {
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            TopicPermissionsDefinition definition = result.DebugInfo.Request.ToObject<TopicPermissionsDefinition>();
            
            definition.Exchange.ShouldBeNullOrEmpty();
            definition.Read.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_topic_permissions_7()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.User(string.Empty);
                    x.VirtualHost(string.Empty);
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();

            TopicPermissionsDefinition definition = result.DebugInfo.Request.ToObject<TopicPermissionsDefinition>();
            
            definition.Exchange.ShouldBe("E4");
            definition.Read.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_topic_permissions_8()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.Configure(c =>
                    {
                        c.UsingReadPattern(string.Empty);
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(4);
            result.DebugInfo.ShouldNotBeNull();

            TopicPermissionsDefinition definition = result.DebugInfo.Request.ToObject<TopicPermissionsDefinition>();
            
            definition.Exchange.ShouldBeNullOrEmpty();
            definition.Read.ShouldBeNullOrEmpty();
            definition.Write.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_topic_permissions_9()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.Configure(c =>
                    {
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(4);
            result.DebugInfo.ShouldNotBeNull();

            TopicPermissionsDefinition definition = result.DebugInfo.Request.ToObject<TopicPermissionsDefinition>();
            
            definition.Exchange.ShouldBeNullOrEmpty();
            definition.Read.ShouldBeNullOrEmpty();
            definition.Write.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_topic_permissions_10()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
            result.DebugInfo.ShouldNotBeNull();

            TopicPermissionsDefinition definition = result.DebugInfo.Request.ToObject<TopicPermissionsDefinition>();
            
            definition.Exchange.ShouldBe("E4");
            definition.Read.ShouldBeNullOrEmpty();
            definition.Write.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_topic_permissions_11()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingReadPattern(".*");
                    });
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
            result.DebugInfo.ShouldNotBeNull();

            TopicPermissionsDefinition definition = result.DebugInfo.Request.ToObject<TopicPermissionsDefinition>();
            
            definition.Exchange.ShouldBe("E4");
            definition.Read.ShouldBe(".*");
            definition.Write.ShouldBeNullOrEmpty();
        }

        [Test]
        public void Verify_can_delete_user_permissions()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Delete(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu7");
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_cannot_delete_user_permissions_1()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Delete(x =>
                {
                    x.User(string.Empty);
                    x.VirtualHost("HareDu7");
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_user_permissions_2()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Delete(x =>
                {
                    x.VirtualHost("HareDu7");
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_user_permissions_3()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Delete(x =>
                {
                    x.User("guest");
                    x.VirtualHost(string.Empty);
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_user_permissions_4()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Delete(x =>
                {
                    x.User("guest");
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_user_permissions_5()
        {
            var container = GetContainerBuilder().Build();
            var result = container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Delete(x =>
                {
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }
    }
}
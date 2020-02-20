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
    using System.Threading.Tasks;
    using Autofac;
    using Core.Extensions;
    using HareDu.Registration;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class UserPermissionsTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_user_permissions()
        {
            var container = GetContainerBuilder("TestData/UserPermissionsInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .GetAll();
            
            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBe(8);
            result.Data[0].ShouldNotBeNull();
            result.Data[0].User.ShouldBe("guest");
            result.Data[0].VirtualHost.ShouldBe("/");
            result.Data[0].Configure.ShouldBe(".*");
            result.Data[0].Write.ShouldBe(".*");
            result.Data[0].Read.ShouldBe(".*");
        }

        [Test]
        public async Task Verify_can_delete_user_permissions()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.User("haredu_user");
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });

            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions_1()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.User(string.Empty);
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions_2()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions_3()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.User("haredu_user");
                    x.Targeting(t => t.VirtualHost(string.Empty));
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions_4()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.User("haredu_user");
                    x.Targeting(t => {});
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions_5()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.Targeting(t => {});
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions_6()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public async Task Verify_can_create_user_permissions()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.User("haredu_user");
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern(".*");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });

            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions_1()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.User(string.Empty);
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern(".*");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions_2()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern(".*");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions_3()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.User("haredu_user");
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern(".*");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                    x.Targeting(t => t.VirtualHost(string.Empty));
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions_4()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.User("haredu_user");
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern(".*");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                    x.Targeting(t => {});
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions_5()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.User("haredu_user");
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern(".*");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions_6()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.User(string.Empty);
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern(".*");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                    x.Targeting(t => t.VirtualHost(string.Empty));
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions_7()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.User(string.Empty);
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern(".*");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                    x.Targeting(t => {});
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions_8()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.User(string.Empty);
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern(".*");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions_9()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern(".*");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                });

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }
    }
}
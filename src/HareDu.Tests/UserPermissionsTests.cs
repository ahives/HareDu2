namespace HareDu.Tests
{
    using Core.Extensions;
    using HareDu.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class UserPermissionsTests :
        HareDuTesting
    {
        [Test]
        public void Should_be_able_to_get_all_user_permissions()
        {
            var container = GetContainerBuilder("TestData/UserPermissionsInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .GetAll()
                .GetResult();
            
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
        public void Verify_can_delete_user_permissions()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.User("haredu_user");
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_cannot_delete_user_permissions_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.User(string.Empty);
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_user_permissions_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_user_permissions_3()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.User("haredu_user");
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_user_permissions_4()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.User("haredu_user");
                    x.Targeting(t => {});
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_user_permissions_5()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.Targeting(t => {});
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_delete_user_permissions_6()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_can_create_user_permissions()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_user_permissions_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_user_permissions_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_user_permissions_3()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_user_permissions_4()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_user_permissions_5()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_user_permissions_6()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_user_permissions_7()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_user_permissions_8()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();

            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();

            UserPermissionsDefinition definition = result.DebugInfo.Request.ToObject<UserPermissionsDefinition>();
            
            definition.Configure.ShouldBe(".*");
            definition.Write.ShouldBe(".*");
            definition.Read.ShouldBe(".*");
        }

        [Test]
        public void Verify_cannot_create_user_permissions_9()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern(".*");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                })
                .GetResult();

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
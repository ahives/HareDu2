namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using HareDu.Extensions;
    using HareDu.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Serialization;

    [TestFixture]
    public class NewUserPermissionsTests :
        HareDuTesting
    {
        [Test]
        public async Task Verify_can_get_all_user_permissions1()
        {
            var services = GetContainerBuilder("TestData/UserPermissionsInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .GetAll();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(8, result.Data.Count);
                Assert.IsNotNull(result.Data[0]);
                Assert.AreEqual("guest", result.Data[0].User);
                Assert.AreEqual("/", result.Data[0].VirtualHost);
                Assert.AreEqual(".*", result.Data[0].Configure);
                Assert.AreEqual(".*", result.Data[0].Write);
                Assert.AreEqual(".*", result.Data[0].Read);
            });
        }

        [Test]
        public async Task Verify_can_get_all_user_permissions2()
        {
            var services = GetContainerBuilder("TestData/UserPermissionsInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetAllUserPermissions();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(8, result.Data.Count);
                Assert.IsNotNull(result.Data[0]);
                Assert.AreEqual("guest", result.Data[0].User);
                Assert.AreEqual("/", result.Data[0].VirtualHost);
                Assert.AreEqual(".*", result.Data[0].Configure);
                Assert.AreEqual(".*", result.Data[0].Write);
                Assert.AreEqual(".*", result.Data[0].Read);
            });
        }

        [Test]
        public async Task Verify_can_delete_user_permissions1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete("haredu_user", "HareDu5");

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.AreEqual(0, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_can_delete_user_permissions2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteUserPermissions("haredu_user", "HareDu5");

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.AreEqual(0, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(string.Empty, "HareDu5");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteUserPermissions(string.Empty, "HareDu5");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete("haredu_user", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteUserPermissions("haredu_user", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions5()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(string.Empty, string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_user_permissions6()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteUserPermissions(string.Empty, string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_can_create_user_permissions1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create("haredu_user", "HareDu5", x =>
                {
                    x.UsingConfigurePattern(".*");
                    x.UsingReadPattern(".*");
                    x.UsingWritePattern(".*");
                });

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.AreEqual(0, result.DebugInfo.Errors.Count);
                Assert.IsNotNull(result.DebugInfo);

                UserPermissionsRequest request = result.DebugInfo.Request.ToObject<UserPermissionsRequest>(Deserializer.Options);
            
                Assert.AreEqual(".*", request.Configure);
                Assert.AreEqual(".*", request.Write);
                Assert.AreEqual(".*", request.Read);
            });
        }

        [Test]
        public async Task Verify_can_create_user_permissions2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateUserPermissions("haredu_user", "HareDu5", x =>
                {
                    x.UsingConfigurePattern(".*");
                    x.UsingReadPattern(".*");
                    x.UsingWritePattern(".*");
                });

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.AreEqual(0, result.DebugInfo.Errors.Count);
                Assert.IsNotNull(result.DebugInfo);

                UserPermissionsRequest request = result.DebugInfo.Request.ToObject<UserPermissionsRequest>(Deserializer.Options);
            
                Assert.AreEqual(".*", request.Configure);
                Assert.AreEqual(".*", request.Write);
                Assert.AreEqual(".*", request.Read);
            });
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(string.Empty, "HareDu5", x =>
                {
                    x.UsingConfigurePattern(".*");
                    x.UsingReadPattern(".*");
                    x.UsingWritePattern(".*");
                });

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
                Assert.IsNotNull(result.DebugInfo);

                UserPermissionsRequest request = result.DebugInfo.Request.ToObject<UserPermissionsRequest>(Deserializer.Options);
            
                Assert.AreEqual(".*", request.Configure);
                Assert.AreEqual(".*", request.Write);
                Assert.AreEqual(".*", request.Read);
            });
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateUserPermissions(string.Empty, "HareDu5", x =>
                {
                    x.UsingConfigurePattern(".*");
                    x.UsingReadPattern(".*");
                    x.UsingWritePattern(".*");
                });

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
                Assert.IsNotNull(result.DebugInfo);

                UserPermissionsRequest request = result.DebugInfo.Request.ToObject<UserPermissionsRequest>(Deserializer.Options);
            
                Assert.AreEqual(".*", request.Configure);
                Assert.AreEqual(".*", request.Write);
                Assert.AreEqual(".*", request.Read);
            });
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create("haredu_user", string.Empty, x =>
                {
                    x.UsingConfigurePattern(".*");
                    x.UsingReadPattern(".*");
                    x.UsingWritePattern(".*");
                });

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
                Assert.IsNotNull(result.DebugInfo);

                UserPermissionsRequest request = result.DebugInfo.Request.ToObject<UserPermissionsRequest>(Deserializer.Options);
            
                Assert.AreEqual(".*", request.Configure);
                Assert.AreEqual(".*", request.Write);
                Assert.AreEqual(".*", request.Read);
            });
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateUserPermissions("haredu_user", string.Empty, x =>
                {
                    x.UsingConfigurePattern(".*");
                    x.UsingReadPattern(".*");
                    x.UsingWritePattern(".*");
                });

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
                Assert.IsNotNull(result.DebugInfo);

                UserPermissionsRequest request = result.DebugInfo.Request.ToObject<UserPermissionsRequest>(Deserializer.Options);
            
                Assert.AreEqual(".*", request.Configure);
                Assert.AreEqual(".*", request.Write);
                Assert.AreEqual(".*", request.Read);
            });
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions5()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(string.Empty, string.Empty, x =>
                {
                    x.UsingConfigurePattern(".*");
                    x.UsingReadPattern(".*");
                    x.UsingWritePattern(".*");
                });

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
                Assert.IsNotNull(result.DebugInfo);

                UserPermissionsRequest request = result.DebugInfo.Request.ToObject<UserPermissionsRequest>(Deserializer.Options);
            
                Assert.AreEqual(".*", request.Configure);
                Assert.AreEqual(".*", request.Write);
                Assert.AreEqual(".*", request.Read);
            });
        }

        [Test]
        public async Task Verify_cannot_create_user_permissions6()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateUserPermissions(string.Empty, string.Empty, x =>
                {
                    x.UsingConfigurePattern(".*");
                    x.UsingReadPattern(".*");
                    x.UsingWritePattern(".*");
                });

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
                Assert.IsNotNull(result.DebugInfo);

                UserPermissionsRequest request = result.DebugInfo.Request.ToObject<UserPermissionsRequest>(Deserializer.Options);
            
                Assert.AreEqual(".*", request.Configure);
                Assert.AreEqual(".*", request.Write);
                Assert.AreEqual(".*", request.Read);
            });
        }
    }
}
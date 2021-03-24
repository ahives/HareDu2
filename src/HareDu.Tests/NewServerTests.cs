namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using HareDu.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    [TestFixture]
    public class NewServerTests :
        HareDuTesting
    {
        [Test]
        public async Task Verify_can_get_all_definitions1()
        {
            var services = GetContainerBuilder("TestData/ServerDefinitionInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Server>()
                .Get();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(8, result.Data.Bindings.Count);
                Assert.AreEqual(11, result.Data.Exchanges.Count);
                Assert.AreEqual(5, result.Data.Queues.Count);
                Assert.AreEqual(3, result.Data.Parameters.Count);
                Assert.AreEqual(8, result.Data.Permissions.Count);
                Assert.AreEqual(2, result.Data.Policies.Count);
                Assert.AreEqual(2, result.Data.Users.Count);
                Assert.AreEqual(9, result.Data.VirtualHosts.Count);
                Assert.AreEqual(5, result.Data.GlobalParameters.Count);
                Assert.AreEqual(3, result.Data.TopicPermissions.Count);
                Assert.AreEqual("3.7.15", result.Data.RabbitMqVersion);
            });
        }

        [Test]
        public async Task Verify_can_get_all_definitions2()
        {
            var services = GetContainerBuilder("TestData/ServerDefinitionInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetServerInformation();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(8, result.Data.Bindings.Count);
                Assert.AreEqual(11, result.Data.Exchanges.Count);
                Assert.AreEqual(5, result.Data.Queues.Count);
                Assert.AreEqual(3, result.Data.Parameters.Count);
                Assert.AreEqual(8, result.Data.Permissions.Count);
                Assert.AreEqual(2, result.Data.Policies.Count);
                Assert.AreEqual(2, result.Data.Users.Count);
                Assert.AreEqual(9, result.Data.VirtualHosts.Count);
                Assert.AreEqual(5, result.Data.GlobalParameters.Count);
                Assert.AreEqual(3, result.Data.TopicPermissions.Count);
                Assert.AreEqual("3.7.15", result.Data.RabbitMqVersion);
            });
        }
    }
}
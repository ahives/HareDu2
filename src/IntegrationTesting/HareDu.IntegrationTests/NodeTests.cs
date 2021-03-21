namespace HareDu.IntegrationTesting.BrokerObjects
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using Extensions;
    using NUnit.Framework;
    using Registration;

    [TestFixture]
    public class NodeTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDuConfiguration($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .AddHareDu()
                .Build();
        }

        [Test]
        public async Task Should_be_able_to_get_all_nodes()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Node>()
                .GetAll()
                .ScreenDump();
        }

        [Test]
        public async Task Should_be_able_to_get_all_memory_usage()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Node>()
                .GetMemoryUsage("rabbit@localhost")
                .ScreenDump();
        }

        [Test]
        public async Task Verify_can_check_if_named_node_healthy()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Node>()
                .GetHealth("rabbit@localhost")
                .ScreenDump();
        }

        [Test]
        public async Task Verify_can_check_if_node_healthy()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Node>()
                .GetHealth();
            
            Console.WriteLine((string) result.DebugInfo.URL);
        }
    }
}
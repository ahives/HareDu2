namespace HareDu.IntegrationTests
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ServerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
                .Build();
        }

        [Test]
        public async Task Should_be_able_to_get_all_definitions()
        {
            Result<ServerInfo> result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Server>()
                .Get()
                .ScreenDump();
        }
    }
}
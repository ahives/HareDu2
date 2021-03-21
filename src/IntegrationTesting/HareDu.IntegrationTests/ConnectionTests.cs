namespace HareDu.IntegrationTesting.BrokerObjects
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Extensions;
    using NUnit.Framework;
    using Registration;

    [TestFixture]
    public class ConnectionTests
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

        [Test, Explicit]
        public async Task Should_be_able_to_get_all_connections()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Connection>()
                .GetAll()
                .ScreenDump();
        }
    }
}
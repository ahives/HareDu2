namespace HareDu.IntegrationTests
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class ConnectionTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/haredu.yaml")
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
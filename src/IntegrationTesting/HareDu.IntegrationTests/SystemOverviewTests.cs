namespace HareDu.IntegrationTests
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class SystemOverviewTests
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
        public async Task Test()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<SystemOverview>()
                .Get()
                .ScreenDump();
        }
    }
}
namespace HareDu.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class ScopedParameterTests
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
        public async Task Should_be_able_to_get_all_scoped_parameters()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .GetAll()
                .ScreenDump();
        }
        
        [Test]
        public async Task Verify_can_create()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<string>(x =>
                {
                    x.Parameter("test", "me");
                    x.Targeting(t =>
                    {
                        t.Component("federation");
                        t.VirtualHost("HareDu");
                    });
                });
            
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        [Test]
        public async Task Verify_can_delete()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter("");
                    x.Targeting(t =>
                    {
                        t.Component("federation");
                        t.VirtualHost("HareDu");
                    });
                });
        }
    }
}
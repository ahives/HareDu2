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
    using Serialization;

    [TestFixture]
    public class ChannelTests
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
                .Object<Channel>()
                .GetAll()
                .ScreenDump();
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }
        
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_channels()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Channel>()
                .GetAll()
                .ScreenDump();
        }
    }
}
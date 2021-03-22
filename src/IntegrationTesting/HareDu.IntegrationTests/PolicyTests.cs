namespace HareDu.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Extensions;
    using NUnit.Framework;
    using Registration;
    using Serialization;

    [TestFixture]
    public class PolicyTests
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
        public async Task Should_be_able_to_get_all_policies()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .GetAll()
                .ScreenDump();
        }
        
        [Test]
        public async Task Verify_can_create_policy()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Create(x =>
                {
                    x.Policy("P5");
                    x.Configure(p =>
                    {
                        p.UsingPattern("^amq.");
                        p.HasPriority(0);
                        p.HasArguments(d =>
                        {
                            d.SetHighAvailabilityMode(HighAvailabilityModes.All);
                            d.SetExpiry(1000);
                        });
                        p.ApplyTo("all");
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
            
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString(Deserializer.Options));
         }

        [Test]
        public async Task Verify_cannot_create_policy()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Create(x =>
                {
                    x.Policy("P4");
                    x.Configure(p =>
                    {
                        p.UsingPattern("^amq.");
                        p.HasPriority(0);
                        p.HasArguments(d =>
                        {
                            d.SetHighAvailabilityMode(HighAvailabilityModes.All);
                            d.SetFederationUpstreamSet("all");
                            d.SetExpiry(1000);
                        });
                        p.ApplyTo("all");
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }

        [Test]
        public async Task Verify_can_delete_policy()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy("P4");
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }
    }
}
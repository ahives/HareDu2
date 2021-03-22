namespace HareDu.IntegrationTests
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
    public class VirtualHostLimitsTests
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
        public async Task Verify_can_get_all_limits()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .GetAll()
                .ScreenDump();
        }

        [Test]
        public async Task Verify_can_get_limits_of_specified_vhost()
        {
            var result = _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .GetAll()
                .Where(x => x.VirtualHostName == "HareDu");

            foreach (var item in result)
            {
                Console.WriteLine("Name: {0}", item.VirtualHostName);

                if (item.Limits.TryGetValue("max-connections", out ulong maxConnections))
                    Console.WriteLine("max-connections: {0}", maxConnections);

                if (item.Limits.TryGetValue("max-queues", out ulong maxQueues))
                    Console.WriteLine("max-queues: {0}", maxQueues);
                
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public async Task Verify_can_define_limits()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("HareDu5");
                    x.Configure(c =>
                    {
                        c.SetMaxQueueLimit(100);
                        c.SetMaxConnectionLimit(1000);
                    });
                });
            
            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }

        [Test]
        public async Task Verify_can_delete_limits()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Delete(x => x.For("HareDu3"));
            
            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }
    }
}
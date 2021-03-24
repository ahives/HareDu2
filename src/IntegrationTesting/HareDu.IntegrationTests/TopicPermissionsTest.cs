namespace HareDu.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using Extensions;
    using NUnit.Framework;
    using Serialization;

    [TestFixture]
    public class TopicPermissionsTest
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
        public async Task Verify_can_get_all_topic_permissions()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .GetAll()
                .ScreenDump();

            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }
        
        [Test]
        public void Verify_can_filter_topic_permissions()
        {
            var result = _container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .GetAll()
                .Where(x => x.VirtualHost == "HareDu");
            
            foreach (var permission in result)
            {
                Console.WriteLine("VirtualHost: {0}", permission.VirtualHost);
                Console.WriteLine("Exchange: {0}", permission.Exchange);
                Console.WriteLine("Read: {0}", permission.Read);
                Console.WriteLine("Write: {0}", permission.Write);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public async Task Verify_can_create_user_permissions()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu");
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                });

            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }

        [Test]
        public async Task Verify_can_delete_user_permissions()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<TopicPermissions>()
                .Delete(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu7");
                });
            
            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }
    }
}
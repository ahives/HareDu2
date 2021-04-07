namespace HareDu.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core;
    using Core.Extensions;
    using Extensions;
    using NUnit.Framework;
    using Serialization;

    [TestFixture]
    public class VirtualHostTests
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
        public async Task Should_be_able_to_get_all_vhosts()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .ScreenDump();
        }

        [Test]
        public async Task Verify_GetAll_HasResult_works()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll();

            Assert.IsTrue(result.HasData);
        }

        [Test]
        public async Task Verify_filtered_GetAll_works()
        {
            var result = _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .GetAll()
                .Where(x => x.Name == "HareDu");

            foreach (var vhost in result)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public async Task Verify_Create_works()
        {
            Result result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Create(x =>
                {
                    x.VirtualHost("HareDu9");
                    x.Configure(c =>
                    {
                        c.WithTracingEnabled();
                        c.Description("My test vhost.");
                        c.Tags(t =>
                        {
                            t.Add("accounts");
                            t.Add("production");
                        });
                    });
                });

            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }

        [Test]
        public async Task Verify_Delete_works()
        {
            Result result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Delete(x => x.VirtualHost("HareDu7"));

            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }

        [Test]
        public async Task Verify_can_start_vhost()
        {
            Result result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHost>()
                .Startup("", x => x.On(""));
            
            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }
    }
}
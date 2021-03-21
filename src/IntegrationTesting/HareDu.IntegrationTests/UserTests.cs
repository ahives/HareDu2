namespace HareDu.IntegrationTesting.BrokerObjects
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
    public class UserTests
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
        public async Task Verify_can_get_all_users()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<User>()
                .GetAll()
                .ScreenDump();
        }
        
        [Test]
        public async Task Verify_can_get_all_users_without_permissions()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<User>()
                .GetAllWithoutPermissions()
                .ScreenDump();
        }
        
        [Test]
        public async Task Verify_can_create()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<User>()
                .Create(x =>
                {
                    x.Username("testuser3");
                    x.Password("testuserpwd3");
                    x.PasswordHash("gkgfjjhfjh");
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                });
            
            Console.WriteLine(result.ToJsonString(Deserializer.Options));
        }


        [Test]
        public async Task Verify_can_delete()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<User>()
                .Delete(x => x.User(""));
        }
    }
}
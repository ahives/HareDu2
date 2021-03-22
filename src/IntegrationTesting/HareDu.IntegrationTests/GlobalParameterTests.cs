namespace HareDu.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core;
    using Core.Extensions;
    using Extensions;
    using Model;
    using NUnit.Framework;
    using Registration;
    using Serialization;

    [TestFixture]
    public class GlobalParameterTests
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
        public async Task Should_be_able_to_get_all_global_parameters()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .GetAll()
                .ScreenDump();
        }
        
        [Test]
        public async Task Verify_can_create_parameter()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter("fake_param2");
                    x.Value("fake_value");
//                    x.Arguments(arg =>
//                    {
//                        arg.Set("arg1", "value1");
//                        arg.Set("arg2", "value2");
//                    });
                });
             
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine((string) result.ToJsonString(Deserializer.Options));
        }
        
        [Test]
        public async Task Verify_can_delete_parameter()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Delete(x => x.Parameter("Fred"));
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine((string) result.ToJsonString(Deserializer.Options));
        }
    }
}
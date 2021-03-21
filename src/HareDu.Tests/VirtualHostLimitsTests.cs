namespace HareDu.Tests
{
    using Core.Extensions;
    using HareDu.Extensions;
    using HareDu.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Serialization;
    using Shouldly;

    [TestFixture]
    public class VirtualHostLimitsTests :
        HareDuTesting
    {
        [Test]
        public void Verify_can_get_all_limits()
        {
            var container = GetContainerBuilder("TestData/VirtualHostLimitsInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .GetAll()
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBe(3);
            result.Data[0].VirtualHostName.ShouldBe("HareDu1");
            result.Data[0].Limits.Count.ShouldBe(2);
            result.Data[0].Limits["max-connections"].ShouldBe<ulong>(10);
            result.Data[0].Limits["max-queues"].ShouldBe<ulong>(10);
        }

        [Test]
        public void Verify_can_define_limits()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("HareDu5");
                    x.Configure(c =>
                    {
                        c.SetMaxQueueLimit(100);
                        c.SetMaxConnectionLimit(1000);
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();

            VirtualHostLimitsDefinition definition = result.DebugInfo.Request.ToObject<VirtualHostLimitsDefinition>(Deserializer.Options);
            
            definition.MaxConnectionLimit.ShouldBe<ulong>(1000);
            definition.MaxQueueLimit.ShouldBe<ulong>(100);
        }

        [Test]
        public void Verify_cannot_define_limits_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost(string.Empty);
                    x.Configure(c =>
                    {
                        c.SetMaxQueueLimit(100);
                        c.SetMaxConnectionLimit(1000);
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            VirtualHostLimitsDefinition definition = result.DebugInfo.Request.ToObject<VirtualHostLimitsDefinition>(Deserializer.Options);
            
            definition.MaxConnectionLimit.ShouldBe<ulong>(1000);
            definition.MaxQueueLimit.ShouldBe<ulong>(100);
        }

        [Test]
        public void Verify_cannot_define_limits_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("FakeVirtualHost");
                    x.Configure(c =>
                    {
                        c.SetMaxConnectionLimit(1000);
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            VirtualHostLimitsDefinition definition = result.DebugInfo.Request.ToObject<VirtualHostLimitsDefinition>(Deserializer.Options);
            
            definition.MaxConnectionLimit.ShouldBe<ulong>(1000);
        }

        [Test]
        public void Verify_cannot_define_limits_3()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("FakeVirtualHost");
                    x.Configure(c =>
                    {
                        c.SetMaxQueueLimit(100);
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            VirtualHostLimitsDefinition definition = result.DebugInfo.Request.ToObject<VirtualHostLimitsDefinition>(Deserializer.Options);
            
            definition.MaxQueueLimit.ShouldBe<ulong>(100);
        }

        [Test]
        public void Verify_cannot_define_limits_4()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("FakeVirtualHost");
                    x.Configure(c =>
                    {
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_define_limits_5()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("FakeVirtualHost");
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_define_limits_6()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
        }

        [Test]
        public void Verify_can_delete_limits()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Delete(x => x.For("HareDu3"))
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_can_delete_limits_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Delete(x => x.For(string.Empty))
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_can_delete_limits_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Delete(x => {})
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }
    }
}
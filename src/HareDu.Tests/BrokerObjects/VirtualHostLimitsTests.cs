// Copyright 2013-2019 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Tests.BrokerObjects
{
    using System.Threading.Tasks;
    using Autofac;
    using Core.Extensions;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class VirtualHostLimitsTests :
        HareDuTesting
    {
        [Test]
        public async Task Verify_can_get_all_limits()
        {
            var container = GetContainerBuilder("TestData/VirtualHostLimitsInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .GetAll();
            
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
        public async Task Verify_can_define_limits()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
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
            
            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();

            VirtualHostLimitsDefinition definition = result.DebugInfo.Request.ToObject<VirtualHostLimitsDefinition>();
            
            definition.MaxConnectionLimit.ShouldBe<ulong>(1000);
            definition.MaxQueueLimit.ShouldBe<ulong>(100);
        }

        [Test]
        public async Task Verify_cannot_define_limits_1()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost(string.Empty);
                    x.Configure(c =>
                    {
                        c.SetMaxQueueLimit(100);
                        c.SetMaxConnectionLimit(1000);
                    });
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            VirtualHostLimitsDefinition definition = result.DebugInfo.Request.ToObject<VirtualHostLimitsDefinition>();
            
            definition.MaxConnectionLimit.ShouldBe<ulong>(1000);
            definition.MaxQueueLimit.ShouldBe<ulong>(100);
        }

        [Test]
        public async Task Verify_cannot_define_limits_2()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("FakeVirtualHost");
                    x.Configure(c =>
                    {
                        c.SetMaxConnectionLimit(1000);
                    });
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            VirtualHostLimitsDefinition definition = result.DebugInfo.Request.ToObject<VirtualHostLimitsDefinition>();
            
            definition.MaxConnectionLimit.ShouldBe<ulong>(1000);
        }

        [Test]
        public async Task Verify_cannot_define_limits_3()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("FakeVirtualHost");
                    x.Configure(c =>
                    {
                        c.SetMaxQueueLimit(100);
                    });
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            VirtualHostLimitsDefinition definition = result.DebugInfo.Request.ToObject<VirtualHostLimitsDefinition>();
            
            definition.MaxQueueLimit.ShouldBe<ulong>(100);
        }

        [Test]
        public async Task Verify_cannot_define_limits_4()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("FakeVirtualHost");
                    x.Configure(c =>
                    {
                    });
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public async Task Verify_cannot_define_limits_5()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("FakeVirtualHost");
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public async Task Verify_cannot_define_limits_6()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Define(x =>
                {
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(3);
        }

        [Test]
        public async Task Verify_can_delete_limits()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Delete(x => x.For("HareDu3"));
            
            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public async Task Verify_can_delete_limits_1()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Delete(x => x.For(string.Empty));
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public async Task Verify_can_delete_limits_2()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Delete(x => {});
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }
    }
}
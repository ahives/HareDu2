// Copyright 2013-2020 Albert L. Hives
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
    using HareDu.Registration;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class PolicyTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_policies()
        {
            var container = GetContainerBuilder("TestData/PolicyInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .GetAll();

            result.HasData.ShouldBeTrue();
            result.HasFaulted.ShouldBeFalse();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBe(2);
            result.Data[0].Name.ShouldBe("P1");
            result.Data[0].VirtualHost.ShouldBe("HareDu");
            result.Data[0].Pattern.ShouldBe("!@#@");
            result.Data[0].AppliedTo.ShouldBe("all");
            result.Data[0].Definition.ShouldNotBeNull();
            result.Data[0].Definition["ha-mode"].ShouldBe("exactly");
            result.Data[0].Definition["ha-sync-mode"].ShouldBe("automatic");
            result.Data[0].Definition["ha-params"].ShouldBe("2");
            result.Data[0].Priority.ShouldBe(0);
        }
        
        [Test]
        public async Task Verify_can_create_policy()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
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
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();

            PolicyDefinition definition = result.DebugInfo.Request.ToObject<PolicyDefinition>();
            
            definition.Pattern.ShouldBe("^amq.");
            definition.Priority.ShouldBe(0);
            definition.Arguments["ha-mode"].ShouldBe("all");
            definition.Arguments["expires"].ShouldBe("1000");
            definition.ApplyTo.ShouldBe("all");
         }

        [Test]
        public async Task Verify_cannot_create_policy_1()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Create(x =>
                {
                    x.Policy(string.Empty);
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
                    x.Target(t => t.VirtualHost(string.Empty));
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.DebugInfo.ShouldNotBeNull();

            PolicyDefinition definition = result.DebugInfo.Request.ToObject<PolicyDefinition>();
            
            definition.Pattern.ShouldBe("^amq.");
            definition.Priority.ShouldBe(0);
            definition.Arguments["ha-mode"].ShouldBe("all");
            definition.Arguments["expires"].ShouldBe("1000");
            definition.ApplyTo.ShouldBe("all");
            
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public async Task Verify_cannot_create_policy_2()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Create(x =>
                {
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
                    x.Target(t => t.VirtualHost(string.Empty));
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.DebugInfo.ShouldNotBeNull();

            PolicyDefinition definition = result.DebugInfo.Request.ToObject<PolicyDefinition>();
            
            definition.Pattern.ShouldBe("^amq.");
            definition.Priority.ShouldBe(0);
            definition.Arguments["ha-mode"].ShouldBe("all");
            definition.Arguments["expires"].ShouldBe("1000");
            definition.ApplyTo.ShouldBe("all");
            
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public async Task Verify_cannot_create_policy_3()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Create(x =>
                {
                    x.Policy(string.Empty);
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
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.DebugInfo.ShouldNotBeNull();

            PolicyDefinition definition = result.DebugInfo.Request.ToObject<PolicyDefinition>();
            
            definition.Pattern.ShouldBe("^amq.");
            definition.Priority.ShouldBe(0);
            definition.Arguments["ha-mode"].ShouldBe("all");
            definition.Arguments["expires"].ShouldBe("1000");
            definition.ApplyTo.ShouldBe("all");
            
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public async Task Verify_cannot_create_policy_4()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Create(x =>
                {
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
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.DebugInfo.ShouldNotBeNull();

            PolicyDefinition definition = result.DebugInfo.Request.ToObject<PolicyDefinition>();
            
            definition.Pattern.ShouldBe("^amq.");
            definition.Priority.ShouldBe(0);
            definition.Arguments["ha-mode"].ShouldBe("all");
            definition.Arguments["expires"].ShouldBe("1000");
            definition.ApplyTo.ShouldBe("all");
            
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public async Task Verify_can_delete_policy()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy("P4");
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public async Task Verify_cannot_delete_policy_1()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy(string.Empty);
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public async Task Verify_cannot_delete_policy_2()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy("P4");
                    x.Target(t => t.VirtualHost(string.Empty));
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public async Task Verify_cannot_delete_policy_3()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy(string.Empty);
                    x.Target(t => t.VirtualHost(string.Empty));
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public async Task Verify_cannot_delete_policy_4()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public async Task Verify_cannot_delete_policy_5()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy("P4");
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public async Task Verify_cannot_delete_policy_6()
        {
            var container = GetContainerBuilder().Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                });
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }
    }
}
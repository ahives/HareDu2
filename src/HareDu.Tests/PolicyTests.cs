namespace HareDu.Tests
{
    using Core.Extensions;
    using HareDu.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Serialization;
    using Shouldly;

    [TestFixture]
    public class PolicyTests :
        HareDuTesting
    {
        [Test]
        public void Should_be_able_to_get_all_policies()
        {
            var container = GetContainerBuilder("TestData/PolicyInfo.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .GetAll()
                .GetResult();

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
        public void Verify_can_create_policy()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();
            
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
        public void Verify_cannot_create_policy_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();
            
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
        public void Verify_cannot_create_policy_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();
            
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
        public void Verify_cannot_create_policy_3()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();
            
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
        public void Verify_cannot_create_policy_4()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
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
                })
                .GetResult();
            
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
        public void Verify_can_delete_policy()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy("P4");
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_cannot_delete_policy_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy(string.Empty);
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_policy_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy("P4");
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_policy_3()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy(string.Empty);
                    x.Targeting(t => t.VirtualHost(string.Empty));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }

        [Test]
        public void Verify_cannot_delete_policy_4()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Targeting(t => t.VirtualHost("HareDu"));
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_policy_5()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy("P4");
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_policy_6()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete(x =>
                {
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
        }
    }
}
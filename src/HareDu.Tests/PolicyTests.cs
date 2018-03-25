﻿namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class PolicyTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Verify_can_create_policy()
        {
            Result result = await Client
                .Factory<Policy>()
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
                            d.SetExpiry(1000);
                        });
                        p.AppliedTo("all");
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
         }

        [Test, Explicit]
        public async Task Verify_cannot_create_policy()
        {
            Result result = await Client
                .Factory<Policy>()
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
                        p.AppliedTo("all");
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }

        [Test, Explicit]
        public async Task Verify_can_delete_policy()
        {
            Result result = await Client
                .Factory<Policy>()
                .Delete(x =>
                {
                    x.Policy("P4");
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }
    }
}
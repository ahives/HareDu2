﻿﻿namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class PolicyTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Verify_can_create_policy()
        {
            Result<PolicyInfo> result = await Client
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
            
//            Console.WriteLine(result.Reason);
//            Assert.AreEqual(HttpStatusCode.Accepted, result.StatusCode);
        }

        [Test, Explicit]
        public async Task Verify_cannot_create_policy()
        {
            Result<PolicyInfo> result = await Client
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
            
//            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test, Explicit]
        public async Task Verify_can_delete_policy()
        {
            Result<PolicyInfo> result = await Client
                .Factory<Policy>()
                .Delete(x =>
                {
                    x.Policy("P4");
                    x.Target(t => t.VirtualHost("HareDu"));
                });
        }
    }
}
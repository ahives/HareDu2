namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class PolicyTests :
        HareDuTestBase
    {
        [Test]
        public async Task Verify_can_create_policy()
        {
            Result result = await Client
                .Factory<Policy>()
                .Create(x =>
                {
                    x.Configure(p =>
                    {
                        p.Resource("P4");
                        p.UsingPattern("^amq.");
                        p.WithPriority(0);
                        p.WithArguments(d =>
                        {
                            d.SetHighAvailabilityMode(HighAvailabilityModes.All);
                            d.SetExpiry(1000);
                        });
                        p.AppliedTo("all");
                    });
                    x.OnVirtualHost("HareDu");
                });
            
            Console.WriteLine(result.Reason);
            Assert.AreEqual(HttpStatusCode.Accepted, result.StatusCode);
        }

        [Test]
        public async Task Verify_cannot_create_policy()
        {
            Result result = await Client
                .Factory<Policy>()
                .Create(x =>
                {
                    x.Configure(p =>
                    {
                        p.Resource("P4");
                        p.UsingPattern("^amq.");
                        p.WithPriority(0);
                        p.WithArguments(d =>
                        {
                            d.SetHighAvailabilityMode(HighAvailabilityModes.All);
                            d.SetFederationUpstreamSet("all");
                            d.SetExpiry(1000);
                        });
                        p.AppliedTo("all");
                    });
                    x.OnVirtualHost("HareDu");
                });
            
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task Verify_can_delete_policy()
        {
            Result result = await Client
                .Factory<Policy>()
                .Delete(x =>
                {
                    x.Resource("P4");
                    x.OnVirtualHost("HareDu");
                });
        }
    }
}
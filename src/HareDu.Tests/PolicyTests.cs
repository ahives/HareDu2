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
                    x.Policy(p =>
                    {
                        p.Name("P3");
                        p.UsingPattern("^amq.");
                        p.WithPriority(0);
                        p.WithArguments(d =>
                        {
                            d.DefineHighAvailabilityMode(HighAvailabilityModes.All);
                            d.DefineExpiry(1000);
                        });
                        p.AppliedTo("all");
                    });
                    x.On("HareDu");
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
                    x.Policy(p =>
                    {
                        p.Name("P4");
                        p.UsingPattern("^amq.");
                        p.WithPriority(0);
                        p.WithArguments(d =>
                        {
                            d.DefineHighAvailabilityMode(HighAvailabilityModes.All);
                            d.DefineFederationUpstreamSet("all");
                            d.DefineExpiry(1000);
                        });
                        p.AppliedTo("all");
                    });
                    x.On("HareDu");
                });
            
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
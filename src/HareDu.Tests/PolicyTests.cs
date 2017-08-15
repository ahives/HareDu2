namespace HareDu.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class PolicyTests :
        HareDuTestBase
    {
        [Test]
        public async Task Test()
        {
            Result result = await Client
                .Factory<Policy>()
                .Create(x =>
                {
                    x.Policy(p =>
                    {
                        p.Name("P1");
                        p.UsingPattern("^amq.");
                        p.WithPriority(0);
                        p.WithArguments(d =>
                        {
                            d.DefineFederationUpstreamSet("all");
                        });
                        p.AppliedTo("all");
                    });
                    x.On("HareDu");
                });
        }
    }
}
namespace HareDu.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class PolicyTests :
        HareDuTestBase
    {
        [Test]
        public void Test()
        {
            var result = Client
                .Factory<Policy>()
                .Create("P1", "HareDu", x =>
                {
                    x.UsePattern("^amq.");
                    x.Priority(0);
                    x.WithArguments(d =>
                    {
                        d.SetFederationUpstreamSet("all");
                    });
                    x.AppliedTo("all");
                });
        }
    }
}
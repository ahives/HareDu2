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
                .Create("HareDu", "P1", x =>
                {
                    x.UsePattern("^amq.");
                    x.Priority(0);
                    x.DefinedAs(new Dictionary<string, object> {{"federation-upstream-set", "all"}});
                    x.AppliedTo("all");
                });
        }
    }
}
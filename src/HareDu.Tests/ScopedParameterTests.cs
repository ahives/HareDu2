namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class ScopedParameterTests :
        HareDuTestBase
    {
        [Test]
        public async Task Verify_can_create()
        {
            Result result = await Client
                .Factory<ScopedParameter>()
                .Create(x =>
                {
                    x.Parameter("test", "me");
                    x.OnComponent("federation");
                    x.OnVirtualHost("HareDu");
                });
            
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        [Test]
        public async Task Test()
        {
            Result result = await Client
                .Factory<ScopedParameter>()
                .Delete(x =>
                {
                    x.Parameter("");
                    x.OnComponent("");
                    x.OnVirtualHost("HareDu");
                });
        }
    }
}
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
                .CreateAsync(x =>
                {
                    x.Parameter("test", "me");
                    x.Targeting(t =>
                    {
                        t.Component("federation");
                        t.VirtualHost("HareDu");
                    });
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
                .DeleteAsync(x =>
                {
                    x.Parameter("");
                    x.Targeting(t =>
                    {
                        t.Component("federation");
                        t.VirtualHost("HareDu");
                    });
                });
        }
    }
}
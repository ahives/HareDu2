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
                    x.OnComponent("federation");
                    x.OnVirtualHost("HareDu");
                    x.SetParameter("test", "me");
                });
            
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
    }
}
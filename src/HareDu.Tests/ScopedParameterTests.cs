namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ScopedParameterTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Verify_can_create()
        {
            Result result = await Client
                .Factory<ScopedParameter>()
                .Create(x =>
                {
                    x.Parameter("test", "me");
                    x.Targeting(t =>
                    {
                        t.Component("federation");
                        t.VirtualHost("HareDu");
                    });
                });
            
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        [Test, Explicit]
        public async Task Test()
        {
            Result result = await Client
                .Factory<ScopedParameter>()
                .Delete(x =>
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
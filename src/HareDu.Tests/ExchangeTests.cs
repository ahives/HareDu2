namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ExchangeTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public void Verify_GetAll_works()
        {
            IEnumerable<ExchangeInfo> result = Client
                .Factory<Exchange>()
                .GetAll()
                .Select(x => x.Data);
                //.Where(x => x.Name == "HareDu");

            foreach (var exchange in result)
            {
                Console.WriteLine("Name: {0}", exchange.Name);
                Console.WriteLine("AutoDelete: {0}", exchange.AutoDelete);
                Console.WriteLine("Internal: {0}", exchange.Internal);
                Console.WriteLine("Durable: {0}", exchange.Durable);
                Console.WriteLine("RoutingType: {0}", exchange.RoutingType);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Verify_Create_works()
        {
            Result result = await Client
                .Factory<Exchange>()
                .Create(x =>
                {
                    x.Exchange("E3");
                    x.Configure(c =>
                    {
                        c.IsDurable();
                        c.IsForInternalUse();
                        c.HasRoutingType(ExchangeRoutingType.Fanout);
//                        c.HasArguments(arg =>
//                        {
//                            arg.Set("", "");
//                        });
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
        }

        [Test, Explicit]
        public async Task Test()
        {
            Result result = await Client
                .Factory<Exchange>()
                .Delete(x =>
                {
                    x.Exchange("E3");
                    x.Targeting(t => t.VirtualHost("HareDu"));
                    x.WithConditions(c => c.IfUnused());
                });
        }
    }
}
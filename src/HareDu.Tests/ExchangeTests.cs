namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ExchangeTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_exchanges()
        {
            var result = await Client
                .Factory<Exchange>()
                .GetAll();

            foreach (var exchange in result.Select(x => x.Data))
            {
                Console.WriteLine("Name: {0}", exchange.Name);
                Console.WriteLine("AutoDelete: {0}", exchange.AutoDelete);
                Console.WriteLine("Internal: {0}", exchange.Internal);
                Console.WriteLine("Durable: {0}", exchange.Durable);
                Console.WriteLine("RoutingType: {0}", exchange.RoutingType);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }

        [Test, Explicit]
        public async Task Verify_can_filter_exchanges()
        {
            Result<IReadOnlyList<ExchangeInfo>> result = await Client
                .Factory<Exchange>()
                .GetAll();

            foreach (var exchange in result.Where(x => x.Name == "amq.*"))
            {
                Console.WriteLine("Name: {0}", exchange.Name);
                Console.WriteLine("AutoDelete: {0}", exchange.AutoDelete);
                Console.WriteLine("Internal: {0}", exchange.Internal);
                Console.WriteLine("Durable: {0}", exchange.Durable);
                Console.WriteLine("RoutingType: {0}", exchange.RoutingType);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }

        [Test, Explicit]
        public async Task Verify_can_create_exchange()
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
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }

        [Test, Explicit]
        public async Task Verify_can_delete_exchange()
        {
            Result result = await Client
                .Factory<Exchange>()
                .Delete(x =>
                {
                    x.Exchange("E3");
                    x.Target(t => t.VirtualHost("HareDu"));
                    x.WithConditions(c => c.IfUnused());
                });
            
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }
    }
}
namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
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
                .Where(x => x.Name == "HareDu");

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
        public async Task Verify_conditional_Delete_works()
        {
            Result result = await Client
                .Factory<Exchange>()
                .Delete("E2", "HareDu", x => x.IfUnused());
 
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
        
        [Test, Explicit]
        public async Task Verify_Delete_works()
        {
            Result result = await Client
                .Factory<Exchange>()
                .Delete("E3", "HareDu");
 
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        [Test, Explicit]
        public async Task Verify_Create_works()
        {
            var args = new Dictionary<string, object> {{"arg1", 5}, {"arg2", true}, {"arg3", "Something"}};

            Result result = await Client
                .Factory<Exchange>()
                .Create("E6", "HareDu", x =>
                {
                    x.IsDurable();
                    x.IsForInternalUse();
                    x.UsingRoutingType(r => r.Fanout());
                    x.WithArguments(args);
                });

            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
    }
}
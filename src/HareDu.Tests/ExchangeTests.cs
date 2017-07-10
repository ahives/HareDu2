namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ExchangeTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Verify_Get_works()
        {
            Result<Exchange> result = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .Factory<ExchangeResource>()
                .Get("E2", "HareDu");
 
            Console.WriteLine("Name: {0}", result.Data.Name);
            Console.WriteLine("AutoDelete: {0}", result.Data.AutoDelete);
            Console.WriteLine("Internal: {0}", result.Data.Internal);
            Console.WriteLine("Durable: {0}", result.Data.Durable);
            Console.WriteLine("RoutingType: {0}", result.Data.RoutingType);
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
        
        [Test, Explicit]
        public async Task Verify_GetAll_works()
        {
            Result<IEnumerable<Exchange>> result = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .Factory<ExchangeResource>()
                .GetAll("HareDu");

            foreach (var exchange in result.Data)
            {
                Console.WriteLine("Name: {0}", exchange.Name);
                Console.WriteLine("AutoDelete: {0}", exchange.AutoDelete);
                Console.WriteLine("Internal: {0}", exchange.Internal);
                Console.WriteLine("Durable: {0}", exchange.Durable);
                Console.WriteLine("RoutingType: {0}", exchange.RoutingType);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
            
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine();
        }
        
        [Test, Explicit]
        public async Task Verify_conditional_Delete_works()
        {
            Result result = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .Factory<ExchangeResource>()
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
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .Factory<ExchangeResource>()
                .Delete("E3", "HareDu");
 
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

    }
}
namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ValueExtensionTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public void Verify_Where_works()
        {
            var vhosts = Client
                .Resource<VirtualHost>()
                .GetAll()
                .Where(x => x.Name == "HareDu");

            foreach (var vhost in vhosts)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public void Verify_Unwrap_works()
        {
            var vhosts = Client
                .Resource<VirtualHost>()
                .GetAll()
                .Unfold();

            foreach (var vhost in vhosts.Data)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public void Verify_Unwravel_works()
        {
            var vhosts = Client
                .Resource<VirtualHost>()
                .GetAll()
                .Select(x => x.Data);

            foreach (var vhost in vhosts)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public void Verify_Any_works()
        {
            bool found = Client
                .Resource<VirtualHost>()
                .GetAll()
                .Any();
            
            Assert.IsTrue(found);
        }

        [Test, Explicit]
        public void Verify_Any_with_predicate_works()
        {
            bool found = Client
                .Resource<VirtualHost>()
                .GetAll()
                .Any(x => x.Name == "HareDu");
            
            Assert.IsTrue(found);
        }
        
        [Test, Explicit]
        public void Verify_FirstOrDefault_works()
        {
            ExchangeInfo exchange = Client
                .Resource<Exchange>()
                .GetAll()
                .Where(x => x.Name == "E2" && x.VirtualHost == "HareDu")
                .FirstOrDefault();
 
            Console.WriteLine("Name: {0}", exchange.Name);
            Console.WriteLine("AutoDelete: {0}", exchange.AutoDelete);
            Console.WriteLine("Internal: {0}", exchange.Internal);
            Console.WriteLine("Durable: {0}", exchange.Durable);
            Console.WriteLine("RoutingType: {0}", exchange.RoutingType);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
        
    }
}
namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualHostTests :
        HareDuTestBase
    {
        [Test]
        public async Task Verify_GetAll_works()
        {
            Result<IEnumerable<VirtualHost>> virtualHosts = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .GetAll();

            foreach (var vhost in virtualHosts.Data)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public async Task Verify_Get_works()
        {
            Result<VirtualHost> virtualHost = await Client
                .Factory<VirtualHostResource>(x => x.Credentials("guest", "guest"))
                .Get("HareDu");

            Console.WriteLine("Name: {0}", virtualHost.Data.Name);
            Console.WriteLine("Tracing: {0}", virtualHost.Data.Tracing);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        class VirtualHostImpl :
            VirtualHost
        {
            public VirtualHostImpl(string name, string tracing)
            {
                Name = name;
                Tracing = tracing;
            }

            public string Name { get; }
            public string Tracing { get; }
        }
    }
}
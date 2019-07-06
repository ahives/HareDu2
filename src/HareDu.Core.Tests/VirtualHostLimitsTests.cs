namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualHostLimitsTests :
        HareDuTestBase
    {
        [Test]
        public async Task Verify_can_get_all_limits()
        {
            var result = await Client
                .Resource<VirtualHostLimits>()
                .GetAll();
            
            foreach (var item in result.Select(x => x.Data))
            {
                Console.WriteLine("Name: {0}", item.VirtualHostName);

                if (item.Limits.TryGetValue("max-connections", out string maxConnections))
                    Console.WriteLine("max-connections: {0}", maxConnections);

                if (item.Limits.TryGetValue("max-queues", out string maxQueues))
                    Console.WriteLine("max-queues: {0}", maxQueues);
                
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public async Task Verify_can_get_limits_of_specified_vhost()
        {
            var result = Client
                .Resource<VirtualHostLimits>()
                .GetAll()
                .Where(x => x.VirtualHostName == "HareDu");

            foreach (var item in result)
            {
                Console.WriteLine("Name: {0}", item.VirtualHostName);

                if (item.Limits.TryGetValue("max-connections", out string maxConnections))
                    Console.WriteLine("max-connections: {0}", maxConnections);

                if (item.Limits.TryGetValue("max-queues", out string maxQueues))
                    Console.WriteLine("max-queues: {0}", maxQueues);
                
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public async Task Verify_can_define_limits()
        {
            var result = await Client
                .Resource<VirtualHostLimits>()
                .Define(x =>
                {
                    x.VirtualHost("HareDu5");
                    x.Configure(c =>
                    {
                        c.SetMaxQueueLimit(100);
                        c.SetMaxConnectionLimit(1000);
                    });
                });
            
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_delete_limits()
        {
            var result = await Client
                .Resource<VirtualHostLimits>()
                .Delete(x => x.For("HareDu3"));
            
            Console.WriteLine(result.ToJsonString());
        }
    }
}
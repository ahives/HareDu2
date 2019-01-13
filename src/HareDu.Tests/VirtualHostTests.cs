namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualHostTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_vhosts()
        {
            Result<IReadOnlyList<VirtualHostInfo>> result = await Client
                .Factory<VirtualHost>()
                .GetAll();

            foreach (var vhost in result.Select(x => x.Data))
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Verify_GetAll_HasResult_works()
        {
            Result<IReadOnlyList<VirtualHostInfo>> result = await Client
                .Factory<VirtualHost>()
                .GetAll();

            Assert.IsTrue(result.HasResult);
        }

        [Test, Explicit]
        public async Task Verify_filtered_GetAll_works()
        {
            IReadOnlyList<VirtualHostInfo> result = Client
                .Factory<VirtualHost>()
                .GetAll()
                .Where(x => x.Name == "HareDu");

            foreach (var vhost in result)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Verify_Create_works()
        {
            Result result = await Client
                .Factory<VirtualHost>()
                .Create(x =>
                {
                    x.VirtualHost("HareDu7");
                    x.Configure(c =>
                    {
                        c.WithTracingEnabled();
                    });
                });

            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_Delete_works()
        {
            Result result = await Client
                .Factory<VirtualHost>()
                .Delete(x => x.VirtualHost("HareDu7"));

            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_get_all_limits()
        {
            Result<IReadOnlyList<VirtualHostLimits>> result = await Client
                .Factory<VirtualHost>()
                .GetAllLimits();
            
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
                .Factory<VirtualHost>()
                .GetAllLimits()
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
            Result result = await Client
                .Factory<VirtualHost>()
                .DefineLimits(x =>
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
            Result result = await Client
                .Factory<VirtualHost>()
                .DeleteLimits(x => x.For("HareDu3"));
            
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_start_vhost()
        {
            Result result = await Client
                .Factory<VirtualHost>()
                .Startup("", x => x.On(""));
            
            Console.WriteLine(result.ToJsonString());
        }
    }
}
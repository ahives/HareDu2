namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualHostTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_vhosts()
        {
            var result = await Client
                .Resource<VirtualHost>()
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
            var result = await Client
                .Resource<VirtualHost>()
                .GetAll();

            Assert.IsTrue(result.HasData);
        }

        [Test, Explicit]
        public async Task Verify_filtered_GetAll_works()
        {
            var result = Client
                .Resource<VirtualHost>()
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
                .Resource<VirtualHost>()
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
                .Resource<VirtualHost>()
                .Delete(x => x.VirtualHost("HareDu7"));

            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_start_vhost()
        {
            Result result = await Client
                .Resource<VirtualHost>()
                .Startup("", x => x.On(""));
            
            Console.WriteLine(result.ToJsonString());
        }
    }
}
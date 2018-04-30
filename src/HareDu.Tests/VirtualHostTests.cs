namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Extensions;
    using Internal.Serialization;
    using Model;
    using Newtonsoft.Json;
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
                    x.VirtualHost("HareDu6");
                    x.Configure(c =>
                    {
                        c.WithTracingEnabled();
                    });
                });

            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }

        [Test, Explicit]
        public async Task Verify_Delete_works()
        {
            Result result = await Client
                .Factory<VirtualHost>()
                .Delete(x => x.VirtualHost("HareDu2"));

            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
    }
}
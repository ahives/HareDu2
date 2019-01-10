namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class TopicPermissionsTest :
        HareDuTestBase
    {
        [Test]
        public async Task Verify_can_get_all_topic_permissions()
        {
            var result = await Client
                .Factory<TopicPermissions>()
                .GetAll();
            
            foreach (var permission in result.Select(x => x.Data))
            {
                Console.WriteLine("VirtualHost: {0}", permission.VirtualHost);
                Console.WriteLine("Exchange: {0}", permission.Exchange);
                Console.WriteLine("Read: {0}", permission.Read);
                Console.WriteLine("Write: {0}", permission.Write);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

            Console.WriteLine(result.ToJsonString());
        }
        
        [Test]
        public void Verify_can_filter_topic_permissions()
        {
            IReadOnlyList<TopicPermissionsInfo> result = Client
                .Factory<TopicPermissions>()
                .GetAll()
                .Where(x => x.VirtualHost == "HareDu");
            
            foreach (var permission in result)
            {
                Console.WriteLine("VirtualHost: {0}", permission.VirtualHost);
                Console.WriteLine("Exchange: {0}", permission.Exchange);
                Console.WriteLine("Read: {0}", permission.Read);
                Console.WriteLine("Write: {0}", permission.Write);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Verify_can_create_user_permissions()
        {
            Result result = await Client
                .Factory<TopicPermissions>()
                .Create(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu");
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                });

            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task TestVerify_can_delete_user_permissions()
        {
            Result result = await Client
                .Factory<TopicPermissions>()
                .Delete(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu7");
                });
            
            Console.WriteLine(result.ToJsonString());
        }
    }
}
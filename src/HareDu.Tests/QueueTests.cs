namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Internal.Serialization;
    using Model;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class QueueTests :
        HareDuTestBase
    {
        [Test]
        public async Task Verify_can_create_queue()
        {
            Queue resource = Client.Factory<Queue>();
            
            Result result = await resource
                .CreateAsync(x =>
                {
                    x.Queue("TestQueue2");
                    x.Configure(c =>
                    {
                        c.IsDurable();
                        c.WithArguments(arg =>
                        {
                            arg.SetQueueExpiration(1000);
                        });
                    });
                    x.Target(t =>
                    {
                        t.Node("MyNode1");
                        t.VirtualHost("HareDu");
                    });
                });
        }

        [Test]
        public async Task Verify_can_get_all()
        {
            Result<IEnumerable<QueueInfo>> result = await Client
                .Factory<Queue>()
                .GetAllAsync();
            
            foreach (var queue in result.Data)
            {
                Console.WriteLine("Name: {0}", queue.Name);
                Console.WriteLine("VirtualHost: {0}", queue.VirtualHost);
                Console.WriteLine("AutoDelete: {0}", queue.AutoDelete);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test]
        public async Task Verify_can_get_all_json()
        {
            Result<IEnumerable<QueueInfo>> result = await Client
                .Factory<Queue>()
                .GetAllAsync();

            Console.WriteLine(result.ToJson());
        }

        [Test]
        public async Task Verify_can_delete_queue()
        {
            Result result = await Client
                .Factory<Queue>()
                .DeleteAsync(x =>
                {
                    x.Queue("");
                    x.Target(l => l.VirtualHost("HareDu"));
                    x.WithConditions(c =>
                    {
                        c.IfUnused();
                        c.IfEmpty();
                    });
                });
        }

        [Test]
        public async Task Verify_can_peek_messages()
        {
            Result result = await Client
                .Factory<Queue>()
                .PeekAsync(x =>
                {
                    x.Queue("");
                    x.Configure(c =>
                    {
                        c.Take(5);
                        c.PutBackWhenFinished();
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
        }

        [Test]
        public async Task Verify_can_empty_queue()
        {
            Result result = await Client
                .Factory<Queue>()
                .EmptyAsync(x =>
                {
                    x.Queue("");
                    x.Target(t => t.VirtualHost("HareDu"));
                });
        }
    }
}
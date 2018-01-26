namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class QueueTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Verify_can_create_queue()
        {
            var client = Client.Factory<Queue>();
            
            Result result = await client
                .Create(x =>
                {
                    x.Queue("TestQueue9");
                    x.Configure(c =>
                    {
                        c.IsDurable();
                        c.HasArguments(arg =>
                        {
                            arg.SetQueueExpiration(1000);
                        });
                    });
                    x.Targeting(t =>
                    {
                        t.VirtualHost("HareDu");
                    });
                });
            
            Console.WriteLine(result.Errors.ToJson());
        }

        [Test, Explicit]
        public async Task Verify_can_get_all()
        {
            IReadOnlyList<QueueInfo> result = Client
                .Factory<Queue>()
                .GetAll()
                .Select(x => x.Data);
            
            foreach (var queue in result)
            {
                Console.WriteLine("Name: {0}", queue.Name);
                Console.WriteLine("VirtualHost: {0}", queue.VirtualHost);
                Console.WriteLine("AutoDelete: {0}", queue.AutoDelete);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Verify_can_get_all_json()
        {
            Result<IEnumerable<QueueInfo>> result = await Client
                .Factory<Queue>()
                .GetAll();

            Console.WriteLine(result.ToJson());
        }

        [Test, Explicit]
        public async Task Verify_can_delete_queue()
        {
            Result result = await Client
                .Factory<Queue>()
                .Delete(x =>
                {
                    x.Queue("");
                    x.Targeting(l => l.VirtualHost("HareDu"));
                    x.When(c =>
                    {
                        c.HasNoConsumers();
                        c.IsEmpty();
                    });
                });
        }

        [Test, Explicit]
        public async Task Verify_can_peek_messages()
        {
            Result<QueueInfo> result = await Client
                .Factory<Queue>()
                .Peek(x =>
                {
                    x.Queue("Queue1");
                    x.Configure(c =>
                    {
                        c.Take(5);
                        c.PutBackWhenFinished();
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
        }

        [Test, Explicit]
        public async Task Verify_can_empty_queue()
        {
            Result result = await Client
                .Factory<Queue>()
                .Empty(x =>
                {
                    x.Queue("");
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
        }
    }
}
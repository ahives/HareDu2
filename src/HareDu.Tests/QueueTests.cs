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
            Result result = await Client
                .Factory<Queue>()
                .Create(x =>
                {
                    x.Queue("TestQueue3");
                    x.Configure(c =>
                    {
                        c.IsDurable();
                        c.HasArguments(arg => { arg.SetQueueExpiration(1000); });
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }

        [Test, Explicit]
        public async Task Should_be_able_to_get_all_queues()
        {
            var result = await Client
                .Factory<Queue>()
                .GetAll();
            
            foreach (var queue in result.Select(x => x.Data))
            {
                Console.WriteLine("Name: {0}", queue.Name);
                Console.WriteLine("VirtualHost: {0}", queue.VirtualHost);
                Console.WriteLine("AutoDelete: {0}", queue.AutoDelete);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }

        [Test, Explicit]
        public async Task Verify_can_get_all_json()
        {
            Result<IEnumerable<QueueInfo>> result = await Client
                .Factory<Queue>()
                .GetAll();
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }

        [Test, Explicit]
        public async Task Verify_can_delete_queue()
        {
            Result result = await Client
                .Factory<Queue>()
                .Delete(x =>
                {
                    x.Queue("TestQueue10");
                    x.Targeting(l => l.VirtualHost("HareDu"));
                    x.When(c =>
                    {
//                        c.HasNoConsumers();
//                        c.IsEmpty();
                    });
                });

//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
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
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
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
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJson());
        }
    }
}
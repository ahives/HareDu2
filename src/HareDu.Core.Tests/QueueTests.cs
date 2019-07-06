namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class QueueTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Verify_can_create_queue()
        {
            var result = await Client
                .Resource<Queue>()
                .Create(x =>
                {
                    x.Queue("TestQueue31");
                    x.Configure(c =>
                    {
                        c.IsDurable();
                        c.HasArguments(arg => { arg.SetQueueExpiration(1000); });
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Should_be_able_to_get_all_queues()
        {
            var result = await Client
                .Resource<Queue>()
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
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_get_all_json()
        {
            var result = await Client
                .Resource<Queue>()
                .GetAll();
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_delete_queue()
        {
            var result = await Client
                .Resource<Queue>()
                .Delete(x =>
                {
                    x.Queue("TestQueue10");
                    x.Target(l => l.VirtualHost("HareDu"));
                    x.When(c =>
                    {
//                        c.HasNoConsumers();
//                        c.IsEmpty();
                    });
                });

//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_peek_messages()
        {
            var result = await Client
                .Resource<Queue>()
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
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_empty_queue()
        {
            var result = await Client
                .Resource<Queue>()
                .Empty(x =>
                {
                    x.Queue("");
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }
    }
}
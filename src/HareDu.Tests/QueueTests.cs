namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class QueueTests :
        HareDuTestBase
    {
        [Test]
        public async Task Test()
        {
            Result result = await Client
                .Factory<Queue>()
                .Create(x =>
                {
                    x.Configure(d =>
                    {
                        d.Name("TestQueue");
                        d.IsDurable();
                        d.WithArguments(arg =>
                        {
                            arg.SetQueueExpiration(1000);
                        });
                    });
                    x.OnNode("MyNode1");
                    x.OnVirtualHost("HareDu");
                });
        }

        [Test]
        public void Test2()
        {
            var result = Client
                .Factory<Queue>()
                .GetAll()
                .Where(x => x.Name == "HareDu");
            
            foreach (var queue in result)
            {
                Console.WriteLine("Name: {0}", queue.Name);
                Console.WriteLine("VirtualHost: {0}", queue.VirtualHost);
                Console.WriteLine("AutoDelete: {0}", queue.AutoDelete);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

        }
    }
}
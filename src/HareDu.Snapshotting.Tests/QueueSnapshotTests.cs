namespace HareDu.Snapshotting.Tests
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Autofac;
    using NUnit.Framework;
    using Observers;

    [TestFixture]
    public class QueueSnapshotTests :
        SnapshotTestBase
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => Client)
                .As<ISnapshotFactory>();
            
//            builder.RegisterModule<MassTransitModule>();

            _container = builder.Build();
        }

        [Test]
        public void Test()
        {
            var broker = Client
                .Snapshot<BrokerQueues>()
                .RegisterObserver(new DefaultQueueSnapshotConsoleLogger())
                .Take()
                .Select(x => x.Data);
            
//            broker.Queues.Where(x => x.Memory.RAM.Total > 0)
        }

        [Test]
        public void Test2()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(typeof(TestClass).FullName));
            
            StringBuilder buffer = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                buffer.Append(data[i].ToString("x2"));
            }
            
            Console.WriteLine(typeof(TestClass).FullName);
            Console.WriteLine(buffer.ToString());
        }

        class TestClass
        {
            
        }
    }
}
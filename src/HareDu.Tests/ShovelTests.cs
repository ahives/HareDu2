namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class ShovelTests :
        SnapshotTestBase
    {
        [Test]
        public async Task Test()
        {
//            Result result = await Client
//                .Resource<Shovel<AMQP091Source, AMQP091Destination>>()
//                .Shovel(x =>
//                {
//                    x.Configure(c =>
//                    {
//                        c.Name("my-shovel");
////                        c.AcknowledgementMode("");
////                        c.ReconnectDelay(100);
//                        c.VirtualHost("%2f");
//                    });
//                    x.Source(s =>
//                    {
//                        s.Uri(u => { u.Builder(b => { b.SetHeartbeat(1); }); });
//                        s.PrefetchCount(2);
//                        s.Queue("my-queue");
//                    });
//                    x.Destination(d =>
//                    {
//                        d.Queue("another-queue");
//                        d.Uri(u =>
//                        {
//                            u.Builder(b =>
//                            {
//                                b.SetHost("remote-server");
////                                b.SetHeartbeat(1);
//                            });
//                        });
//                    });
//                });
//            
//            Console.WriteLine(result.ToJsonString());
        }
    }
}
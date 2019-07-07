namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class ClusterSnapshotTests :
        SnapshotTestBase
    {
        [Test]
        public async Task Test()
        {
            var summary = await Client
                .Snapshot<ClusterSnapshot>()
                .Get();
            
            Console.WriteLine("Acknowledged => Total: {0}, Rate: {1}", summary.Queue.Acknowledged.Total, summary.Queue.Acknowledged.Rate);
            Console.WriteLine("Delivered => Total: {0}, Rate: {1}", summary.Queue.Delivered.Total, summary.Queue.Delivered.Rate);
            Console.WriteLine("Gets => Total: {0}, Rate: {1}", summary.Queue.Gets.Total, summary.Queue.Gets.Rate);
            Console.WriteLine("Published => Total: {0}, Rate: {1}", summary.Queue.Published.Total, summary.Queue.Published.Rate);
            Console.WriteLine("Redelivered => Total: {0}, Rate: {1}", summary.Queue.Redelivered.Total, summary.Queue.Redelivered.Rate);
            Console.WriteLine("DeliveryGets => Total: {0}, Rate: {1}", summary.Queue.DeliveryGets.Total, summary.Queue.DeliveryGets.Rate);
            Console.WriteLine("DeliveredWithoutAcknowledgement => Total: {0}, Rate: {1}", summary.Queue.DeliveredWithoutAcknowledgement.Total, summary.Queue.DeliveredWithoutAcknowledgement.Rate);
            Console.WriteLine("GetsWithoutAcknowledgement => Total: {0}, Rate: {1}", summary.Queue.GetsWithoutAcknowledgement.Total, summary.Queue.GetsWithoutAcknowledgement.Rate);
            Console.WriteLine("Disk Reads => Total: {0}, Rate: {1}", summary.IO.Reads.Total, summary.IO.Reads.Rate);
            Console.WriteLine("Disk Writes => Total: {0}, Rate: {1}", summary.IO.Writes.Total, summary.IO.Writes.Rate);
            
//            Console.WriteLine(": {0}", summary.IOStatusDetails);
        }
    }
}
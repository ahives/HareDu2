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
            var snapshot = await Client
                .Snapshot<ClusterSnapshot>()
                .Get();
            
//            snapshot.Nodes[0].IO.Seeks
            Console.WriteLine("Acknowledged => Total: {0}, Rate: {1}", snapshot.Queue.Acknowledged.Total, snapshot.Queue.Acknowledged.Rate);
            Console.WriteLine("Delivered => Total: {0}, Rate: {1}", snapshot.Queue.Delivered.Total, snapshot.Queue.Delivered.Rate);
            Console.WriteLine("Gets => Total: {0}, Rate: {1}", snapshot.Queue.Gets.Total, snapshot.Queue.Gets.Rate);
            Console.WriteLine("Published => Total: {0}, Rate: {1}", snapshot.Queue.Published.Total, snapshot.Queue.Published.Rate);
            Console.WriteLine("Redelivered => Total: {0}, Rate: {1}", snapshot.Queue.Redelivered.Total, snapshot.Queue.Redelivered.Rate);
            Console.WriteLine("DeliveryGets => Total: {0}, Rate: {1}", snapshot.Queue.DeliveryGets.Total, snapshot.Queue.DeliveryGets.Rate);
            Console.WriteLine("DeliveredWithoutAcknowledgement => Total: {0}, Rate: {1}", snapshot.Queue.DeliveredWithoutAcknowledgement.Total, snapshot.Queue.DeliveredWithoutAcknowledgement.Rate);
            Console.WriteLine("GetsWithoutAcknowledgement => Total: {0}, Rate: {1}", snapshot.Queue.GetsWithoutAcknowledgement.Total, snapshot.Queue.GetsWithoutAcknowledgement.Rate);
            Console.WriteLine("Disk Reads => Total: {0}, Rate: {1}", snapshot.Nodes[0].IO.Reads.Total, snapshot.Nodes[0].IO.Reads.Rate);
            Console.WriteLine("Disk Writes => Total: {0}, Rate: {1}", snapshot.Nodes[0].IO.Writes.Total, snapshot.Nodes[0].IO.Writes.Rate);
            
//            Console.WriteLine(": {0}", summary.IOStatusDetails);
        }
    }
}
namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class HealthCheckTests :
        HareDuTestBase
    {
        [Test]
        public async Task Verify_IsHealthy_works()
        {
            Result<ServerHealth> result = await Client
                .Factory<Server>()
                .CheckUp(x =>
                {
                    x.Name("HareDu");
                    x.Type(HealthCheckType.VirtualHost);
                });

            Console.WriteLine("Status: {0}", result.Data.Status);
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
        
        [Test]
        public async Task Verify_GetAllNodes_works()
        {
            Result<IEnumerable<NodeInfo>> result = await Client
                .Factory<Cluster>()
                .GetNodes();

            Assert.IsTrue(result.HasValue());
            
            foreach (var node in result.Data)
            {
                Console.WriteLine("OperatingSystemPid: {0}", node.OperatingSystemPid);
                Console.WriteLine("TotalFileDescriptors: {0}", node.TotalFileDescriptors);
                Console.WriteLine("MemoryUsedDetailsRate: {0}", node.MemoryUsedRate.Rate);
                Console.WriteLine("FileDescriptorUsedDetailsRate: {0}", node.FileDescriptorUsedRate.Rate);
                Console.WriteLine("StatusCode: {0}", result.StatusCode);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
        
        [Test]
        public async Task Verify_GetChannels_works()
        {
            Result<IEnumerable<ChannelInfo>> result = await Client
                .Factory<Node>()
                .GetChannels();

            foreach (var node in result.Data)
            {
                Console.WriteLine("Name: {0}", node.Name);
                Console.WriteLine("PrefetchCount: {0}", node.PrefetchCount);
//                Console.WriteLine("MemoryUsedDetailsRate: {0}", node.MemoryUsedRate.Rate);
//                Console.WriteLine("FileDescriptorUsedDetailsRate: {0}", node.FileDescriptorUsedRate.Rate);
                Console.WriteLine("StatusCode: {0}", result.StatusCode);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
        
        [Test]
        public async Task Verify_GetClusterDetails_works()
        {
            Result<ClusterInfo> result = await Client
                .Factory<Cluster>()
                .GetDetails();

            Console.WriteLine("ClusterName: {0}", result.Data.ClusterName);
//            Console.WriteLine("TotalQueues: {0}", result.Data.ClusterObjects.TotalQueues);
//            Console.WriteLine("TotalConsumers: {0}", result.Data.ClusterObjects.TotalConsumers);
//            Console.WriteLine("TotalExchanges: {0}", result.Data.ClusterObjects.TotalExchanges);
            Console.WriteLine("RabbitMqVersion: {0}", result.Data.RabbitMqVersion);
            Console.WriteLine("****************************************************");
            Console.WriteLine();

//            foreach (var listener in result.Data.Listeners)
//            {
//                Console.WriteLine("Node: {0}", listener.Node);
//                Console.WriteLine("IPAddress: {0}", listener.IPAddress);
//                Console.WriteLine("Port: {0}", listener.Port);
//                Console.WriteLine("****************************************************");
//                Console.WriteLine();
//            }
        }
    }
}
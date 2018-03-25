namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
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
                .HealthCheck(x =>
                {
                    x.VirtualHost("HareDu");
                });

            Console.WriteLine("Status: {0}", result.Data.Status);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
        
        [Test]
        public async Task Verify_GetAllNodes_works()
        {
            Result<IReadOnlyList<NodeInfo>> result = await Client
                .Factory<Cluster>()
                .GetNodes();
            
            foreach (var node in result.Select(x => x.Data))
            {
                Console.WriteLine("OperatingSystemPid: {0}", node.OperatingSystemPid);
                Console.WriteLine("TotalFileDescriptors: {0}", node.TotalFileDescriptors);
                Console.WriteLine("MemoryUsedDetailsRate: {0}", node.MemoryUsedRate.Rate);
                Console.WriteLine("FileDescriptorUsedDetailsRate: {0}", node.FileDescriptorUsedRate.Rate);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
        
        [Test]
        public async Task Verify_GetChannels_works()
        {
            Result<IReadOnlyList<ChannelInfo>> result = await Client
                .Factory<Node>()
                .GetChannels();

            foreach (var node in result.Select(x => x.Data))
            {
                Console.WriteLine("Name: {0}", node.Name);
                Console.WriteLine("PrefetchCount: {0}", node.PrefetchCount);
//                Console.WriteLine("MemoryUsedDetailsRate: {0}", node.MemoryUsedRate.Rate);
//                Console.WriteLine("FileDescriptorUsedDetailsRate: {0}", node.FileDescriptorUsedRate.Rate);
//                Console.WriteLine("StatusCode: {0}", result.StatusCode);
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
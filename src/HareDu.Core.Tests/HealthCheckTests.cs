namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class HealthCheckTests :
        HareDuTestBase
    {
        [Test]
        public async Task Verify_IsHealthy_works()
        {
//            Result<Model.ServerHealth> result = await Client
//                .Resource<ServerHealth>()
//                .GetDetails(x =>
//                {
//                    x.VirtualHost("HareDu");
//                });
//
//            Console.WriteLine("Status: {0}", result.Data.Status);
//            Console.WriteLine("****************************************************");
//            Console.WriteLine();
        }
        
        [Test]
        public async Task Verify_GetClusterDetails_works()
        {
//            Result<ClusterInfo> result = await Client
//                .Resource<Cluster>()
//                .GetDetails();
//
//            Console.WriteLine("ClusterName: {0}", result.Data.ClusterName);
////            Console.WriteLine("TotalQueues: {0}", result.Data.ClusterObjects.TotalQueues);
////            Console.WriteLine("TotalConsumers: {0}", result.Data.ClusterObjects.TotalConsumers);
////            Console.WriteLine("TotalExchanges: {0}", result.Data.ClusterObjects.TotalExchanges);
//            Console.WriteLine("RabbitMqVersion: {0}", result.Data.RabbitMqVersion);
//            Console.WriteLine("****************************************************");
//            Console.WriteLine();

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
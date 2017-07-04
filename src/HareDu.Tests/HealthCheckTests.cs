namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class HealthCheckTests :
        HareDuTestBase
    {
        [Test]
        public async Task Test()
        {
            Result<VirtualHostHealthCheck> result = await Client
                .Factory<ServerResource>(x => x.Credentials("guest", "guest"))
                .IsVirtualHostHealthy("HareDu2");

            Console.WriteLine("Status: {0}", result.Data.Status);
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
        
        [Test]
        public async Task Verify_IsNodeHealthy_works()
        {
            Result<NodeHealthCheck> result = await Client
                .Factory<ServerResource>(x => x.Credentials("guest", "guest"))
                .IsNodeHealthy();

            Console.WriteLine("Status: {0}", result.Data.Status);
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine("****************************************************");
            Console.WriteLine();
        }
        
        [Test]
        public async Task Verify_GetAllNodes_works()
        {
            Result<IEnumerable<Node>> result = await Client
                .Factory<ServerResource>(x => x.Credentials("guest", "guest"))
                .GetAllNodes();

            foreach (var node in result.Data)
            {
                Console.WriteLine("OperatingSystemPid: {0}", node.OperatingSystemPid);
                Console.WriteLine("TotalFileDescriptors: {0}", node.TotalFileDescriptors);
                Console.WriteLine("StatusCode: {0}", result.StatusCode);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
    }
}
namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class ClusterTests :
        HareDuTestBase
    {
        
        [Test, Explicit]
        public async Task Verify_can_check_if_named_node_healthy()
        {
            Result result = await Client
                .Resource<Cluster>()
                .NodeHealthy("rabbit@localhost");
            
            Console.WriteLine(result.DebugInfo.URL);
        }

        [Test, Explicit]
        public async Task Verify_can_check_if_node_healthy()
        {
            Result result = await Client
                .Resource<Cluster>()
                .NodeHealthy();
            
            Console.WriteLine(result.DebugInfo.URL);
        }

    }
}
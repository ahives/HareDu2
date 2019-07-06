namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Model;
    using NUnit.Framework;

    [TestFixture]
    public class NodeHealthTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Verify_can_check_if_named_node_healthy()
        {
            var result = await Client
                .Resource<NodeHealth>()
                .GetDetails("rabbit@localhost");

            if (result.HasData)
            {
                foreach (var info in result.Data)
                {
                    Console.WriteLine("Reason: {0}", info.Reason);
                    Console.WriteLine("Status: {0}", info.Status);
                    Console.WriteLine("****************************************************");
                    Console.WriteLine();
                }
            }
//            Console.WriteLine(result.DebugInfo.URL);
        }

        [Test, Explicit]
        public async Task Verify_can_check_if_node_healthy()
        {
            Result<NodeHealthInfo> result = await Client
                .Resource<NodeHealth>()
                .GetDetails();
            
            Console.WriteLine(result.DebugInfo.URL);
        }
    }
}
namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using Internal.Resources;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class NodeTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_nodes()
        {
            Result<NodeInfo> result = await Client
                .Resource<Node>()
                .GetAll();
            
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
    }
}
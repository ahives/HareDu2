namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Core.Model;
    using NUnit.Framework;

    [TestFixture]
    public class NodeTests :
        ResourceTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_nodes()
        {
            var result = await Client
                .Resource<Node>()
                .GetAll();
            
            foreach (var node in result.Select(x => x.Data))
            {
                Console.WriteLine("OperatingSystemPid: {0}", node.OperatingSystemProcessId);
                Console.WriteLine("TotalFileDescriptors: {0}", node.TotalFileDescriptors);
                Console.WriteLine("MemoryUsedDetailsRate: {0}", node.MemoryUsedDetails.Rate);
                Console.WriteLine("FileDescriptorUsedDetailsRate: {0}", node.FileDescriptorUsedDetails.Rate);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
    }
}
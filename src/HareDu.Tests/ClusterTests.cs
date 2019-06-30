namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class ClusterTests :
        HareDuTestBase
    {
        [Test]
        public async Task Test()
        {
            var result = await Client
                .Resource<Cluster>()
                .GetDetails();

            if (result.HasData)
            {
                foreach (ClusterInfo info in result.Data)
                {
                    Console.WriteLine("FileDescriptorUsedDetailsRate: {0}", info.Node);

                    foreach (var context in info.Contexts)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Node: {0}", context.Node);
                        Console.WriteLine("Description: {0}", context.Description);
                        Console.WriteLine("Port: {0}", context.Port);
                    }
                    Console.WriteLine("****************************************************");
                    Console.WriteLine();
                }
            }
        }
    }
}
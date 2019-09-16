namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Core.Model;
    using NUnit.Framework;

    [TestFixture]
    public class ClusterTests :
        ResourceTestBase
    {
        [Test]
        public async Task Test()
        {
            var result = await Client
                .Resource<Cluster>()
                .GetDetails();

            if (result.HasData)
            {
                ClusterInfo info = result.Select(x => x.Data);
                Console.WriteLine("Node: {0}", info.Node);
                Console.WriteLine("ManagementVersion: {0}", info.ManagementVersion);
                Console.WriteLine("ClusterName: {0}", info.ClusterName);
                Console.WriteLine("Erlang Verion: {0}", info.ErlangVersion);
                Console.WriteLine("Erlang Verion (Full): {0}", info.ErlangFullVersion);
                Console.WriteLine("RMQ Version: {0}", info.RabbitMqVersion);
                Console.WriteLine("Rates Mode: {0}", info.RatesMode);
                Console.WriteLine("Total Disk Reads: {0}", info.MessageStats.TotalDiskReads);
//                    Console.WriteLine(": {0}", info);

                foreach (var context in info.Contexts)
                {
                    Console.WriteLine();
                    Console.WriteLine("Description: {0}", context.Description);
                    Console.WriteLine("Port: {0}", context.Port);
                    Console.WriteLine("Path: {0}", context.Path);
                }
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
    }
}
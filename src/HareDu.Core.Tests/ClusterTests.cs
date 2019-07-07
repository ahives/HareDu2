namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Core;
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
                foreach (ClusterInfo info in result.Data)
                {
                    Console.WriteLine("Node: {0}", info.Node);
                    Console.WriteLine("ManagementVersion: {0}", info.ManagementVersion);
                    Console.WriteLine("ClusterName: {0}", info.ClusterName);
                    Console.WriteLine("Erlang Verion: {0}", info.ErlangVerion);
                    Console.WriteLine("Erlang Verion (Full): {0}", info.ErlangFullVerion);
                    Console.WriteLine("RMQ Version: {0}", info.RabbitMqVersion);
                    Console.WriteLine("Rates Mode: {0}", info.RatesMode);
                    Console.WriteLine("Total Disk Reads: {0}", info.MessageStats.TotalDiskReads);
//                    Console.WriteLine("Rate Of Disk Reads: {0}", info.MessageStats.RateOfDiskReads.Rate);
//                    Console.WriteLine("Rate Of Message Delivered: {0}", info.MessageStats?.RateOfMessageDelivered?.Rate);
//                    Console.WriteLine(": {0}", info.MessageStats);
//                    Console.WriteLine(": {0}", info.MessageStats);
//                    Console.WriteLine(": {0}", info);
//                    Console.WriteLine(": {0}", info);
//                    Console.WriteLine(": {0}", info);
//                    Console.WriteLine(": {0}", info);
//                    Console.WriteLine(": {0}", info);
//                    Console.WriteLine(": {0}", info);
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
}
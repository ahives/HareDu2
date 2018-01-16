namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class NodeTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public void Test()
        {
            IEnumerable<ChannelInfo> nodes = Client
                .Factory<Node>()
                .GetChannels()
                .Safely();
            
            foreach (var node in nodes)
            {
                Console.WriteLine("Name: {0}", node.Name);
                Console.WriteLine("VirtualHost: {0}", node.VirtualHost);
                Console.WriteLine("Host: {0}", node.Host);
                Console.WriteLine("TotalChannels: {0}", node.TotalChannels);
                Console.WriteLine("TotalConsumers: {0}", node.TotalConsumers);
//                Console.WriteLine("Host: {0}", node);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
    }
}
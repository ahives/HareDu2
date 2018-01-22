namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class NodeTests :
        HareDuTestBase
    {
        [Test]
        public void Test()
        {
            IEnumerable<ChannelInfo> result = Client
                .Factory<Node>()
                .GetChannels()
                .Select(x => x.Data);
//                .Unwrap();
            
//            Assert.IsTrue(result.TryGetValue(out var nodes));
            
            foreach (var node in result)
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
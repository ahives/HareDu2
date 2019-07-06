namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Core.Model;
    using NUnit.Framework;

    [TestFixture]
    public class ConsumerTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_consumers()
        {
            var result = await Client
                .Resource<Consumer>()
                .GetAll();

            if (result.HasData)
            {
                foreach (var consumer in result.Select(x => x.Data))
                {
                    Console.WriteLine("AcknowledgementRequired: {0}", consumer.AcknowledgementRequired);
                    Console.WriteLine("Arguments: {0}", consumer.Arguments);
                    Console.WriteLine("Name: {0}", consumer.ChannelDetails?.Name);
                    Console.WriteLine("ConnectionName: {0}", consumer.ChannelDetails?.ConnectionName);
                    Console.WriteLine("Node: {0}", consumer.ChannelDetails?.Node);
                    Console.WriteLine("Number: {0}", consumer.ChannelDetails?.Number);
                    Console.WriteLine("PeerHost: {0}", consumer.ChannelDetails?.PeerHost);
                    Console.WriteLine("PeerPort: {0}", consumer.ChannelDetails?.PeerPort);
                    Console.WriteLine("User: {0}", consumer.ChannelDetails?.User);
                    Console.WriteLine("ConsumerTag: {0}", consumer.ConsumerTag);
                    Console.WriteLine("Exclusive: {0}", consumer.Exclusive);
                    Console.WriteLine("PreFetchCount: {0}", consumer.PreFetchCount);
                    Console.WriteLine("Name: {0}", consumer.QueueConsumerDetails?.Name);
                    Console.WriteLine("VirtualHost: {0}", consumer.QueueConsumerDetails?.VirtualHost);
                    Console.WriteLine("****************************************************");
                    Console.WriteLine();
                }
            }
        }
    }
}
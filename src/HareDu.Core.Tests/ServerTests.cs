namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Model;
    using NUnit.Framework;

    [TestFixture]
    public class ServerTests :
        ResourceTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_definitions()
        {
            Result<ServerDefinitionInfo> result = await Client
                .Resource<Server>()
                .GetDefinition();

            if (result.HasData)
            {
                var definition = result.Data[0];
                
                Console.WriteLine("RabbitMqVersion: {0}", definition.RabbitMqVersion);
                
                foreach (var binding in definition.Bindings)
                {
                    Console.WriteLine("Destination: {0}", binding.Destination);
                    Console.WriteLine("DestinationType: {0}", binding.DestinationType);
                    Console.WriteLine("PropertiesKey: {0}", binding.PropertiesKey);
                    Console.WriteLine("RoutingKey: {0}", binding.RoutingKey);
                    Console.WriteLine("Source: {0}", binding.Source);
                    Console.WriteLine("VirtualHost: {0}", binding.VirtualHost);
                }
                
                foreach (var exchange in definition.Exchanges)
                {
                    Console.WriteLine("AutoDelete: {0}", exchange.AutoDelete);
                    Console.WriteLine("Durable: {0}", exchange.Durable);
                    Console.WriteLine("Internal: {0}", exchange.Internal);
                    Console.WriteLine("Name: {0}", exchange.Name);
                    Console.WriteLine("RoutingType: {0}", exchange.RoutingType);
                    Console.WriteLine("VirtualHost: {0}", exchange.VirtualHost);
                }
                
                foreach (var queue in definition.Queues)
                {
                    Console.WriteLine("AutoDelete: {0}", queue.AutoDelete);
                    Console.WriteLine("Durable: {0}", queue.Durable);
                    Console.WriteLine("Internal: {0}", queue.Consumers);
                    Console.WriteLine("Name: {0}", queue.Name);
                    Console.WriteLine("ConsumerUtilization: {0}", queue.ConsumerUtilization);
                    Console.WriteLine("Exclusive: {0}", queue.Exclusive);
                    Console.WriteLine("IdleSince: {0}", queue.IdleSince.ToString());
                    Console.WriteLine("HeadMessageTimestamp: {0}", queue.HeadMessageTimestamp.ToString());
                    Console.WriteLine("MinorGarbageCollection: {0}", queue.GC?.MinorGarbageCollection);
                    Console.WriteLine("MinimumHeapSize: {0}", queue.GC?.MinimumHeapSize);
                    Console.WriteLine("MinimumBinaryVirtualHeapSize: {0}", queue.GC?.MinimumBinaryVirtualHeapSize);
                    Console.WriteLine("MaximumHeapSize: {0}", queue.GC?.MaximumHeapSize);
                    Console.WriteLine("FullSweepAfter: {0}", queue.GC?.FullSweepAfter);
                    Console.WriteLine("ExclusiveConsumerTag: {0}", queue.ExclusiveConsumerTag);
                    Console.WriteLine("ConsumerUtilization: {0}", queue.ConsumerUtilization);
                    Console.WriteLine("VirtualHost: {0}", queue.VirtualHost);
                    Console.WriteLine("UnacknowledgedDeliveredMessagesInRam: {0}", queue.UnacknowledgedMessagesInRam);
                    Console.WriteLine("UnacknowledgedDeliveredMessages: {0}", queue.UnacknowledgedMessages);
                    Console.WriteLine("TotalMessagePagedOut: {0}", queue.TotalMessagesPagedOut);
                    Console.WriteLine("TotalMessageBytesPagedOut: {0}", queue.TotalMessageBytesPagedOut);
                    Console.WriteLine("TotalBytesOfMessagesReadyForDelivery: {0}", queue.TotalBytesOfMessagesReadyForDelivery);
                    Console.WriteLine("TotalBytesOfMessagesDeliveredButUnacknowledged: {0}", queue.TotalBytesOfMessagesDeliveredButUnacknowledged);
                    Console.WriteLine("TotalBytesOfAllMessages: {0}", queue.TotalBytesOfAllMessages);
                    Console.WriteLine("State: {0}", queue.State);
                    Console.WriteLine("RecoverableSlaves: {0}", queue.RecoverableSlaves);
                    Console.WriteLine("RateOfReductions: {0}", queue.RateOfReductions?.Rate);
                    Console.WriteLine("RateOfMessagesUnacknowledged: {0}", queue.RateOfUnacknowledgedMessages?.Rate);
                    Console.WriteLine("RateOfMessagesReady: {0}", queue.RateOfReadyMessages?.Rate);
                    Console.WriteLine("RateOfMessage: {0}", queue.RateOfMessage?.Rate);
                    Console.WriteLine("Policy: {0}", queue.Policy);
                    Console.WriteLine("TotalTargetRam: {0}", queue.MessageRates?.TotalTargetRam);
                    Console.WriteLine("Q4: {0}", queue.MessageRates?.Q4);
                    Console.WriteLine("Q3: {0}", queue.MessageRates?.Q3);
                    Console.WriteLine("Q2: {0}", queue.MessageRates?.Q2);
                    Console.WriteLine("Q1: {0}", queue.MessageRates?.Q1);
                    Console.WriteLine("NextSequenceId: {0}", queue.MessageRates?.NextSequenceId);
                    Console.WriteLine("Mode: {0}", queue.MessageRates?.Mode);
                    Console.WriteLine("Length: {0}", queue.MessageRates?.Length);
                    Console.WriteLine("AvgIngressRate: {0}", queue.MessageRates?.AvgIngressRate);
                    Console.WriteLine("AvgEgressRate: {0}", queue.MessageRates?.AvgEgressRate);
                    Console.WriteLine("AvgAcknowledgementIngressRate: {0}", queue.MessageRates?.AvgAcknowledgementIngressRate);
                    Console.WriteLine("AvgAcknowledgementEgressRate: {0}", queue.MessageRates?.AvgAcknowledgementEgressRate);
                    Console.WriteLine("MessageBytesPersisted: {0}", queue.MessageBytesPersisted);
                    Console.WriteLine("MessageBytesInRam: {0}", queue.MessageBytesInRam);
                    Console.WriteLine("Memory: {0}", queue.Memory);
                }

                foreach (var user in definition.Users)
                {
                    Console.WriteLine("HashingAlgorithm: {0}", user.HashingAlgorithm);
                    Console.WriteLine("PasswordHash: {0}", user.PasswordHash);
                    Console.WriteLine("Tags: {0}", user.Tags);
                    Console.WriteLine("Username: {0}", user.Username);
                }

                foreach (var virtualHost in definition.VirtualHosts)
                {
                    Console.WriteLine("Name: {0}", virtualHost.Name);
                    Console.WriteLine("Tracing: {0}", virtualHost.Tracing);
                }
                
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
        
    }
}
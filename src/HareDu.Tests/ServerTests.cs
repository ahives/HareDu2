// Copyright 2013-2019 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using NUnit.Framework;

    [TestFixture]
    public class ServerTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_definitions()
        {
            var container = GetContainerBuilder("TestData/ServerDefinitionInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Server>()
                .GetDefinition();
            
            Assert.IsFalse(result.HasFaulted);
            Assert.IsTrue(result.HasData);
            Assert.IsNotNull(result.Data);

            if (result.HasData)
            {
                Console.WriteLine("RabbitMqVersion: {0}", result.Data.RabbitMqVersion);
                
                foreach (var binding in result.Data.Bindings)
                {
                    Console.WriteLine("Destination: {0}", binding.Destination);
                    Console.WriteLine("DestinationType: {0}", binding.DestinationType);
                    Console.WriteLine("PropertiesKey: {0}", binding.PropertiesKey);
                    Console.WriteLine("RoutingKey: {0}", binding.RoutingKey);
                    Console.WriteLine("Source: {0}", binding.Source);
                    Console.WriteLine("VirtualHost: {0}", binding.VirtualHost);
                }
                
                foreach (var exchange in result.Data.Exchanges)
                {
                    Console.WriteLine("AutoDelete: {0}", exchange.AutoDelete);
                    Console.WriteLine("Durable: {0}", exchange.Durable);
                    Console.WriteLine("Internal: {0}", exchange.Internal);
                    Console.WriteLine("Name: {0}", exchange.Name);
                    Console.WriteLine("RoutingType: {0}", exchange.RoutingType);
                    Console.WriteLine("VirtualHost: {0}", exchange.VirtualHost);
                }
                
                foreach (var queue in result.Data.Queues)
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
                    Console.WriteLine("TotalMessageBytesPagedOut: {0}", queue.MessageBytesPagedOut);
                    Console.WriteLine("TotalBytesOfMessagesReadyForDelivery: {0}", queue.TotalBytesOfMessagesReadyForDelivery);
                    Console.WriteLine("TotalBytesOfMessagesDeliveredButUnacknowledged: {0}", queue.TotalBytesOfMessagesDeliveredButUnacknowledged);
                    Console.WriteLine("TotalBytesOfAllMessages: {0}", queue.TotalBytesOfAllMessages);
                    Console.WriteLine("State: {0}", queue.State);
                    Console.WriteLine("RecoverableSlaves: {0}", queue.RecoverableSlaves);
                    Console.WriteLine("RateOfReductions: {0}", queue.ReductionRate?.Rate);
                    Console.WriteLine("RateOfMessagesUnacknowledged: {0}", queue.UnackedMessageRate?.Rate);
                    Console.WriteLine("RateOfMessagesReady: {0}", queue.ReadyMessageRate?.Rate);
                    Console.WriteLine("RateOfMessage: {0}", queue.MessageRate?.Rate);
                    Console.WriteLine("Policy: {0}", queue.Policy);
                    Console.WriteLine("TotalTargetRam: {0}", queue.BackingQueueStatus?.TargetTotalMessagesInRAM);
                    Console.WriteLine("Q4: {0}", queue.BackingQueueStatus?.Q4);
                    Console.WriteLine("Q3: {0}", queue.BackingQueueStatus?.Q3);
                    Console.WriteLine("Q2: {0}", queue.BackingQueueStatus?.Q2);
                    Console.WriteLine("Q1: {0}", queue.BackingQueueStatus?.Q1);
                    Console.WriteLine("NextSequenceId: {0}", queue.BackingQueueStatus?.NextSequenceId);
                    Console.WriteLine("Mode: {0}", queue.BackingQueueStatus?.Mode);
                    Console.WriteLine("Length: {0}", queue.BackingQueueStatus?.Length);
                    Console.WriteLine("AvgIngressRate: {0}", queue.BackingQueueStatus?.AvgIngressRate);
                    Console.WriteLine("AvgEgressRate: {0}", queue.BackingQueueStatus?.AvgEgressRate);
                    Console.WriteLine("AvgAcknowledgementIngressRate: {0}", queue.BackingQueueStatus?.AvgAcknowledgementIngressRate);
                    Console.WriteLine("AvgAcknowledgementEgressRate: {0}", queue.BackingQueueStatus?.AvgAcknowledgementEgressRate);
                    Console.WriteLine("MessageBytesPersisted: {0}", queue.MessageBytesPersisted);
                    Console.WriteLine("MessageBytesInRam: {0}", queue.MessageBytesInRam);
                    Console.WriteLine("Memory: {0}", queue.Memory);
                }

                foreach (var user in result.Data.Users)
                {
                    Console.WriteLine("HashingAlgorithm: {0}", user.HashingAlgorithm);
                    Console.WriteLine("PasswordHash: {0}", user.PasswordHash);
                    Console.WriteLine("Tags: {0}", user.Tags);
                    Console.WriteLine("Username: {0}", user.Username);
                }

                foreach (var virtualHost in result.Data.VirtualHosts)
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
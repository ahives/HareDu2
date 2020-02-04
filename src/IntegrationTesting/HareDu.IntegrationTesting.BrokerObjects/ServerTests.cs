// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.IntegrationTesting.BrokerObjects
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core;
    using Model;
    using NUnit.Framework;
    using Registration;

    [TestFixture]
    public class ServerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuModule>();

            _container = builder.Build();
        }

        [Test]
        public async Task Should_be_able_to_get_all_definitions()
        {
            Result<ServerDefinitionInfo> result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Server>()
                .GetDefinition();

            if (result.HasData)
            {
                Console.WriteLine((string) "RabbitMqVersion: {0}", (object) result.Data.RabbitMqVersion);
                
                foreach (var binding in result.Data.Bindings)
                {
                    Console.WriteLine((string) "Destination: {0}", (object) binding.Destination);
                    Console.WriteLine((string) "DestinationType: {0}", (object) binding.DestinationType);
                    Console.WriteLine((string) "PropertiesKey: {0}", (object) binding.PropertiesKey);
                    Console.WriteLine((string) "RoutingKey: {0}", (object) binding.RoutingKey);
                    Console.WriteLine((string) "Source: {0}", (object) binding.Source);
                    Console.WriteLine((string) "VirtualHost: {0}", (object) binding.VirtualHost);
                }
                
                foreach (var exchange in result.Data.Exchanges)
                {
                    Console.WriteLine((string) "AutoDelete: {0}", (object) exchange.AutoDelete);
                    Console.WriteLine((string) "Durable: {0}", (object) exchange.Durable);
                    Console.WriteLine((string) "Internal: {0}", (object) exchange.Internal);
                    Console.WriteLine((string) "Name: {0}", (object) exchange.Name);
                    Console.WriteLine((string) "RoutingType: {0}", (object) exchange.RoutingType);
                    Console.WriteLine((string) "VirtualHost: {0}", (object) exchange.VirtualHost);
                }
                
                foreach (var queue in result.Data.Queues)
                {
                    Console.WriteLine((string) "AutoDelete: {0}", (object) queue.AutoDelete);
                    Console.WriteLine((string) "Durable: {0}", (object) queue.Durable);
                    Console.WriteLine((string) "Internal: {0}", (object) queue.Consumers);
                    Console.WriteLine((string) "Name: {0}", (object) queue.Name);
                    Console.WriteLine((string) "ConsumerUtilization: {0}", (object) queue.ConsumerUtilization);
                    Console.WriteLine((string) "Exclusive: {0}", (object) queue.Exclusive);
                    Console.WriteLine((string) "IdleSince: {0}", (object) queue.IdleSince.ToString());
                    Console.WriteLine((string) "HeadMessageTimestamp: {0}", (object) queue.HeadMessageTimestamp.ToString());
                    Console.WriteLine((string) "MinorGarbageCollection: {0}", (object) queue.GC?.MinorGarbageCollection);
                    Console.WriteLine((string) "MinimumHeapSize: {0}", (object) queue.GC?.MinimumHeapSize);
                    Console.WriteLine((string) "MinimumBinaryVirtualHeapSize: {0}", (object) queue.GC?.MinimumBinaryVirtualHeapSize);
                    Console.WriteLine((string) "MaximumHeapSize: {0}", (object) queue.GC?.MaximumHeapSize);
                    Console.WriteLine((string) "FullSweepAfter: {0}", (object) queue.GC?.FullSweepAfter);
                    Console.WriteLine((string) "ExclusiveConsumerTag: {0}", (object) queue.ExclusiveConsumerTag);
                    Console.WriteLine((string) "ConsumerUtilization: {0}", (object) queue.ConsumerUtilization);
                    Console.WriteLine((string) "VirtualHost: {0}", (object) queue.VirtualHost);
                    Console.WriteLine((string) "UnacknowledgedDeliveredMessagesInRam: {0}", (object) queue.UnacknowledgedMessagesInRam);
                    Console.WriteLine((string) "UnacknowledgedDeliveredMessages: {0}", (object) queue.UnacknowledgedMessages);
                    Console.WriteLine((string) "TotalMessagePagedOut: {0}", (object) queue.TotalMessagesPagedOut);
                    Console.WriteLine((string) "TotalMessageBytesPagedOut: {0}", (object) queue.MessageBytesPagedOut);
                    Console.WriteLine((string) "TotalBytesOfMessagesReadyForDelivery: {0}", (object) queue.TotalBytesOfMessagesReadyForDelivery);
                    Console.WriteLine((string) "TotalBytesOfMessagesDeliveredButUnacknowledged: {0}", (object) queue.TotalBytesOfMessagesDeliveredButUnacknowledged);
                    Console.WriteLine((string) "TotalBytesOfAllMessages: {0}", (object) queue.TotalBytesOfAllMessages);
                    Console.WriteLine((string) "State: {0}", (object) queue.State);
                    Console.WriteLine((string) "RecoverableSlaves: {0}", (object) queue.RecoverableSlaves);
                    Console.WriteLine((string) "RateOfReductions: {0}", (object) queue.ReductionRate?.Rate);
                    Console.WriteLine((string) "RateOfMessagesUnacknowledged: {0}", (object) queue.UnackedMessageRate?.Rate);
                    Console.WriteLine((string) "RateOfMessagesReady: {0}", (object) queue.ReadyMessageRate?.Rate);
                    Console.WriteLine((string) "RateOfMessage: {0}", (object) queue.MessageRate?.Rate);
                    Console.WriteLine((string) "Policy: {0}", (object) queue.Policy);
                    Console.WriteLine((string) "TotalTargetRam: {0}", (object) queue.BackingQueueStatus?.TargetTotalMessagesInRAM);
                    Console.WriteLine((string) "Q4: {0}", (object) queue.BackingQueueStatus?.Q4);
                    Console.WriteLine((string) "Q3: {0}", (object) queue.BackingQueueStatus?.Q3);
                    Console.WriteLine((string) "Q2: {0}", (object) queue.BackingQueueStatus?.Q2);
                    Console.WriteLine((string) "Q1: {0}", (object) queue.BackingQueueStatus?.Q1);
                    Console.WriteLine((string) "NextSequenceId: {0}", (object) queue.BackingQueueStatus?.NextSequenceId);
                    Console.WriteLine((string) "Mode: {0}", (object) queue.BackingQueueStatus?.Mode);
                    Console.WriteLine((string) "Length: {0}", (object) queue.BackingQueueStatus?.Length);
                    Console.WriteLine((string) "AvgIngressRate: {0}", (object) queue.BackingQueueStatus?.AvgIngressRate);
                    Console.WriteLine((string) "AvgEgressRate: {0}", (object) queue.BackingQueueStatus?.AvgEgressRate);
                    Console.WriteLine((string) "AvgAcknowledgementIngressRate: {0}", (object) queue.BackingQueueStatus?.AvgAcknowledgementIngressRate);
                    Console.WriteLine((string) "AvgAcknowledgementEgressRate: {0}", (object) queue.BackingQueueStatus?.AvgAcknowledgementEgressRate);
                    Console.WriteLine((string) "MessageBytesPersisted: {0}", (object) queue.MessageBytesPersisted);
                    Console.WriteLine((string) "MessageBytesInRam: {0}", (object) queue.MessageBytesInRam);
                    Console.WriteLine((string) "Memory: {0}", (object) queue.Memory);
                }

                foreach (var user in result.Data.Users)
                {
                    Console.WriteLine((string) "HashingAlgorithm: {0}", (object) user.HashingAlgorithm);
                    Console.WriteLine((string) "PasswordHash: {0}", (object) user.PasswordHash);
                    Console.WriteLine((string) "Tags: {0}", (object) user.Tags);
                    Console.WriteLine((string) "Username: {0}", (object) user.Username);
                }

                foreach (var virtualHost in result.Data.VirtualHosts)
                {
                    Console.WriteLine((string) "Name: {0}", (object) virtualHost.Name);
                    Console.WriteLine((string) "Tracing: {0}", (object) virtualHost.Tracing);
                }
                
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
    }
}
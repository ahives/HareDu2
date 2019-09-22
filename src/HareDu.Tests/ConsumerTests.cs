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
    using AutofacIntegration;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class ConsumerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuModule>();

            _container = builder.Build();
        }

        [Test, Explicit]
        public async Task Should_be_able_to_get_all_consumers()
        {
            var result = await _container.Resolve<IRmqObjectFactory>()
                .Object<Consumer>()
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
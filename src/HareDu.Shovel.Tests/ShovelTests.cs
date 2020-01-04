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
namespace HareDu.Shovel.Tests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class ShovelTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuShovelModule>();

            _container = builder.Build();
        }

        [Test]
        public async Task Test()
        {
            Result result = await _container.Resolve<IShovelFactory>()
                .Shovel(x =>
                {
                    x.Configure(c =>
                    {
                        c.Name("my-shovel");
//                        c.AcknowledgementMode("");
//                        c.ReconnectDelay(100);
                        c.VirtualHost("test-vhost");
                    });
                    x.Source(c =>
                    {
                        c.Protocol(ShovelProtocol.AMQP_091);
                        c.Uri(u => { u.Build(b => { b.SetHeartbeat(1); }); });
                        c.PrefetchCount(2);
                        c.Exchange("TestExchange", "cool-exchange");
                        c.Queue("my-queue");
                    });
                    x.Destination(c =>
                    {
                        c.Protocol(ShovelProtocol.AMQP_091);
                        c.Queue("another-queue");
                        c.Uri(u =>
                        {
                            u.Build(b =>
                            {
                                b.SetHost("remote-server");
//                                b.SetHeartbeat(1);
                            });
                        });
                    });
                });
            
            Console.WriteLine(result.ToJsonString());
        }
    }
}
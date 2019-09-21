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
namespace HareDu.Shovel.Tests
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class ShovelTests :
        ResourceTestBase
    {
        [Test]
        public async Task Test()
        {
            Result result = await Client
                .Object<Shovel<AMQP091Source, AMQP091Destination>>()
                .Shovel(x =>
                {
                    x.Configure(c =>
                    {
                        c.Name("my-shovel");
//                        c.AcknowledgementMode("");
//                        c.ReconnectDelay(100);
                        c.VirtualHost("%2f");
                    });
                    x.Source(s =>
                    {
                        s.Uri(u => { u.Builder(b => { b.SetHeartbeat(1); }); });
                        s.PrefetchCount(2);
                        s.Queue("my-queue");
                    });
                    x.Destination(d =>
                    {
                        d.Queue("another-queue");
                        d.Uri(u =>
                        {
                            u.Builder(b =>
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
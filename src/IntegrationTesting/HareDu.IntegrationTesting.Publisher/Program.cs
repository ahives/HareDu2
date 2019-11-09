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
namespace HareDu.IntegrationTesting.Publisher
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using MassTransit;

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Publisher");
            var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                var host = config.Host(new Uri("rabbitmq://localhost/HareDu"), x =>
                {
                    x.Username("guest");
                    x.Password("guest");
                });
            });

            bus.Start();

            await Task.WhenAll(Enumerable.Range(0,50000).Select(x => bus.Publish<FakeMessage>(new FakeMessageImpl())));
            
            bus.Stop();
        }

        
        class FakeMessageImpl :
            FakeMessage
        {
            public FakeMessageImpl()
            {
                CorrelationId = NewId.NextGuid();
                Timestamp = DateTime.Now;
            }

            public Guid CorrelationId { get; }
            public DateTime Timestamp { get; }
        }
    }
}
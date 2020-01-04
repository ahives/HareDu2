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
namespace HareDu.IntegrationTesting.Consumer
{
    using Autofac;
    using MassTransit;
    using MassTransit.Log4NetIntegration;

    public class MassTransitModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new MyBusSettings("guest", "guest", "consumer_queue"))
                .As<BusSettings>();
            
            builder.Register(context =>
                {
                    var settings = context.Resolve<BusSettings>();

                    var bus = Bus.Factory.CreateUsingRabbitMq(config =>
                    {
                        var host = config.Host(settings.HostAddress, x =>
                        {
                            x.Username(settings.Username);
                            x.Password(settings.Password);
                        });
                        
                        config.UseLog4Net();

                        config.ReceiveEndpoint(host, settings.QueueName, x =>
                        {
                            x.PrefetchCount = 64;
                            x.Consumer<FakeConsumer>(context);
                        });
                    });

                    return bus;
                })
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();

            builder.RegisterType<FakeConsumer>()
                .AsSelf();
        }
    }
}
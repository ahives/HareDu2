namespace Publisher
{
    using System;
    using Autofac;
    using Consumer;
    using MassTransit;
    using MassTransit.Log4NetIntegration;

    public class MassTransitModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new MyBusSettings("guest", "guest", "TestQueue"))
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

    public class MyBusSettings :
        BusSettings
    {
        public MyBusSettings(string username, string password, string queueName)
        {
            Username = username;
            Password = password;
            QueueName = queueName;
            HostAddress = new Uri("rabbitmq://localhost/Machete/");
        }

        public string Username { get; }
        public string Password { get; }
        public string QueueName { get; }
        public Uri HostAddress { get; }
    }

    public interface BusSettings
    {
        string Username { get; }
        
        string Password { get; }
        
        string QueueName { get; }
        
        Uri HostAddress { get; }
    }
}
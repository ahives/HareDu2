namespace Publisher
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using MassTransit;

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Publisher");
            var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                var host = config.Host(new Uri("rabbitmq://localhost/TestVirtualHost"), x =>
                {
                    x.Username("guest");
                    x.Password("guest");
                });
            });

            bus.Start();

            await Task.WhenAll(Enumerable.Range(0,1000000).Select(x => bus.Publish<FakeMessage>(new FakeMessageImpl())));
            
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
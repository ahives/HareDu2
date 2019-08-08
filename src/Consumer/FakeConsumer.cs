namespace Consumer
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using MassTransit;

    public class FakeConsumer :
        IConsumer<FakeMessage>
    {
        public async Task Consume(ConsumeContext<FakeMessage> context) => Console.WriteLine("Consumed message");
    }
}
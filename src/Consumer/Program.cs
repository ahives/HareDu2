namespace Consumer
{
    using System;
    using Autofac;
    using MassTransit;
    using Publisher;

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<MassTransitModule>();

            var container = builder.Build();

            var bus = container.Resolve<IBusControl>();
            bus.Start();
            
            Console.WriteLine("Consumer Started");
        }
    }
}
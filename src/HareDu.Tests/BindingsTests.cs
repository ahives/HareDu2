namespace HareDu.Tests
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class BindingsTests :
        HareDuTestBase
    {
        [Test]
        public async Task Test()
        {
            var result = await Client
                .Factory<Binding>()
                .GetAll();
            
            foreach (var binding in result.Data)
            {
                Console.WriteLine("VirtualHost: {0}", binding.VirtualHost);
                Console.WriteLine("Source: {0}", binding.Source);
                Console.WriteLine("Destination: {0}", binding.Destination);
                Console.WriteLine("DestinationType: {0}", binding.DestinationType);
                Console.WriteLine("RoutingKey: {0}", binding.RoutingKey);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
            
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine();
        }

        [Test]
        public async Task Verify_can_add_arguments()
        {
            Result result = await Client
                .Factory<Binding>()
                .Create(x =>
                {
                    x.Binding(b =>
                    {
                        b.Bind("E2");
                        b.To("Q1");
                        b.WithRoutingKey("*.");
                        b.WithArguments(a =>
                        {
                            a.Set("arg1", "value1");
                        });
                    });
                    x.On("HareDu");
                }, BindingType.Exchange);

        }
    }
}
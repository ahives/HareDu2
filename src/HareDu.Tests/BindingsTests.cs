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
        public async Task Verify_can_get_all_bindings()
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
                        b.Source("E2");
                        b.Destination("Q1");
                        b.Type(BindingType.Exchange);
                    });
                    x.Configure(c =>
                    {
                        c.WithRoutingKey("*.");
                        c.WithArguments(arg =>
                        {
                            arg.Set("arg1", "value1");
                        });
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
        }

        [Test]
        public async Task Verify_can_delete_binding()
        {
            Result result = await Client
                .Factory<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name("%2A.");
                        b.Source("E2");
                        b.Destination("Q4");
                        b.Type(BindingType.Queue);
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
        }
    }
}
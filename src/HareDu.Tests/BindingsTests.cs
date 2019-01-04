namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class BindingsTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_bindings()
        {
            var result = await Client
                .Factory<Binding>()
                .GetAll();
            
            foreach (var binding in result.Select(x => x.Data))
            {
                Console.WriteLine("VirtualHost: {0}", binding.VirtualHost);
                Console.WriteLine("Source: {0}", binding.Source);
                Console.WriteLine("Destination: {0}", binding.Destination);
                Console.WriteLine("DestinationType: {0}", binding.DestinationType);
                Console.WriteLine("RoutingKey: {0}", binding.RoutingKey);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_add_arguments()
        {
            Result<BindingInfo> result = await Client
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
                        c.HasRoutingKey("*.");
                        c.HasArguments(arg =>
                        {
                            arg.Set("arg1", "value1");
                        });
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
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
            
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }
    }
}
namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using HareDu.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Serialization;

    [TestFixture]
    public class OperatorPolicyTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_policies1()
        {
            var services = GetContainerBuilder("TestData/OperatorPolicyInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<OperatorPolicy>()
                .GetAll();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasData);
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(1, result.Data.Count);
                Assert.AreEqual("test", result.Data[0].Name);
                Assert.AreEqual("TestHareDu", result.Data[0].VirtualHost);
                Assert.AreEqual(".*", result.Data[0].Pattern);
                Assert.AreEqual("queues", result.Data[0].AppliedTo);
                Assert.IsNotNull(result.Data[0].Definition);
                Assert.AreEqual(100, result.Data[0].Definition["max-length"]);
                Assert.AreEqual(0, result.Data[0].Priority);
            });
        }

        [Test]
        public async Task Should_be_able_to_get_all_policies2()
        {
            var services = GetContainerBuilder("TestData/OperatorPolicyInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetAllOperatorPolicies();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasData);
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(1, result.Data.Count);
                Assert.AreEqual("test", result.Data[0].Name);
                Assert.AreEqual("TestHareDu", result.Data[0].VirtualHost);
                Assert.AreEqual(".*", result.Data[0].Pattern);
                Assert.AreEqual("queues", result.Data[0].AppliedTo);
                Assert.IsNotNull(result.Data[0].Definition);
                Assert.AreEqual(100, result.Data[0].Definition["max-length"]);
                Assert.AreEqual(0, result.Data[0].Priority);
            });
        }
        
        [Test]
        public async Task Verify_can_create_policy1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<OperatorPolicy>()
                .Create("policy1", "^amq.", "HareDu", x =>
                {
                    x.SetExpiry(1000);
                    x.SetDeliveryLimit(5);
                }, OperatorPolicyAppliedTo.Queues, 0);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);

                OperatorPolicyRequest request = result.DebugInfo.Request.ToObject<OperatorPolicyRequest>();

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual(5, request.Arguments["delivery-limit"]);
                Assert.AreEqual(1000, request.Arguments["expires"]);
                Assert.AreEqual(OperatorPolicyAppliedTo.Queues, request.ApplyTo);
            });
        }
        
        [Test]
        public async Task Verify_can_create_policy2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateOperatorPolicy("policy1", "^amq.", "HareDu", x =>
                {
                    x.SetExpiry(1000);
                    x.SetDeliveryLimit(5);
                }, OperatorPolicyAppliedTo.Queues, 0);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);

                OperatorPolicyRequest request = result.DebugInfo.Request.ToObject<OperatorPolicyRequest>();

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual(5, request.Arguments["delivery-limit"]);
                Assert.AreEqual(1000, request.Arguments["expires"]);
                Assert.AreEqual(OperatorPolicyAppliedTo.Queues, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_cannot_create_policy1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<OperatorPolicy>()
                .Create(string.Empty, "^amq.", string.Empty, x =>
                {
                    x.SetExpiry(1000);
                    x.SetDeliveryLimit(5);
                }, OperatorPolicyAppliedTo.Queues, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                OperatorPolicyRequest request = result.DebugInfo.Request.ToObject<OperatorPolicyRequest>();

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual(5, request.Arguments["delivery-limit"]);
                Assert.AreEqual(1000, request.Arguments["expires"]);
                Assert.AreEqual(OperatorPolicyAppliedTo.Queues, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_cannot_create_policy2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateOperatorPolicy(string.Empty, "^amq.", string.Empty, x =>
                {
                    x.SetExpiry(1000);
                    x.SetDeliveryLimit(5);
                }, OperatorPolicyAppliedTo.Queues, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                OperatorPolicyRequest request = result.DebugInfo.Request.ToObject<OperatorPolicyRequest>();

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual(5, request.Arguments["delivery-limit"]);
                Assert.AreEqual(1000, request.Arguments["expires"]);
                Assert.AreEqual(OperatorPolicyAppliedTo.Queues, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_cannot_create_policy3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<OperatorPolicy>()
                .Create(string.Empty, "^amq.", string.Empty, x =>
                {
                    x.SetExpiry(1000);
                    x.SetDeliveryLimit(5);
                }, OperatorPolicyAppliedTo.Queues, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                OperatorPolicyRequest request = result.DebugInfo.Request.ToObject<OperatorPolicyRequest>();

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual(5, request.Arguments["delivery-limit"]);
                Assert.AreEqual(1000, request.Arguments["expires"]);
                Assert.AreEqual(OperatorPolicyAppliedTo.Queues, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_cannot_create_policy4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                    .CreateOperatorPolicy(string.Empty, "^amq.", string.Empty, x =>
                    {
                        x.SetExpiry(1000);
                        x.SetDeliveryLimit(5);
                    }, OperatorPolicyAppliedTo.Queues, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                OperatorPolicyRequest request = result.DebugInfo.Request.ToObject<OperatorPolicyRequest>();

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual(5, request.Arguments["delivery-limit"]);
                Assert.AreEqual(1000, request.Arguments["expires"]);
                Assert.AreEqual(OperatorPolicyAppliedTo.Queues, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_cannot_create_policy5()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<OperatorPolicy>()
                .Create(string.Empty, "^amq.", string.Empty, x =>
                {
                    x.SetExpiry(1000);
                    x.SetDeliveryLimit(5);
                }, OperatorPolicyAppliedTo.Queues, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                OperatorPolicyRequest request = result.DebugInfo.Request.ToObject<OperatorPolicyRequest>();

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual(5, request.Arguments["delivery-limit"]);
                Assert.AreEqual(1000, request.Arguments["expires"]);
                Assert.AreEqual(OperatorPolicyAppliedTo.Queues, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_cannot_create_policy6()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateOperatorPolicy(string.Empty, "^amq.", string.Empty, x =>
                {
                    x.SetExpiry(1000);
                    x.SetDeliveryLimit(5);
                }, OperatorPolicyAppliedTo.Queues, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                OperatorPolicyRequest request = result.DebugInfo.Request.ToObject<OperatorPolicyRequest>();

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual(5, request.Arguments["delivery-limit"]);
                Assert.AreEqual(1000, request.Arguments["expires"]);
                Assert.AreEqual(OperatorPolicyAppliedTo.Queues, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_can_delete_policy1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<OperatorPolicy>()
                .Delete("P4", "HareDu");
            
            Assert.IsFalse(result.HasFaulted);
        }

        [Test]
        public async Task Verify_can_delete_policy2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteOperatorPolicy("P4", "HareDu");
            
            Assert.IsFalse(result.HasFaulted);
        }

        [Test]
        public async Task Verify_cannot_delete_policy1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<OperatorPolicy>()
                .Delete(string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_policy2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteOperatorPolicy(string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_policy3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<OperatorPolicy>()
                .Delete("P4", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_policy4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteOperatorPolicy("P4", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_policy5()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<OperatorPolicy>()
                .Delete(string.Empty, string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_policy6()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteOperatorPolicy(string.Empty, string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_policy7()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<OperatorPolicy>()
                .Delete(string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_policy8()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteOperatorPolicy(string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_policy9()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<OperatorPolicy>()
                .Delete("P4", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete_policy10()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteOperatorPolicy("P4", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }
    }
}
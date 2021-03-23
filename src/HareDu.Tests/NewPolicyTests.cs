namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using HareDu.Extensions;
    using HareDu.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Serialization;

    [TestFixture]
    public class NewPolicyTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_policies1()
        {
            var services = GetContainerBuilder("TestData/PolicyInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .GetAll();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasData);
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(2, result.Data.Count);
                Assert.AreEqual("P1", result.Data[0].Name);
                Assert.AreEqual("HareDu", result.Data[0].VirtualHost);
                Assert.AreEqual("!@#@", result.Data[0].Pattern);
                Assert.AreEqual("all", result.Data[0].AppliedTo);
                Assert.IsNotNull(result.Data[0].Definition);
                Assert.AreEqual("exactly", result.Data[0].Definition["ha-mode"]);
                Assert.AreEqual("automatic", result.Data[0].Definition["ha-sync-mode"]);
                Assert.AreEqual("2", result.Data[0].Definition["ha-params"]);
                Assert.AreEqual(0, result.Data[0].Priority);
            });
        }
        
        [Test]
        public async Task Should_be_able_to_get_all_policies2()
        {
            var services = GetContainerBuilder("TestData/PolicyInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetAllPolicies();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasData);
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(2, result.Data.Count);
                Assert.AreEqual("P1", result.Data[0].Name);
                Assert.AreEqual("HareDu", result.Data[0].VirtualHost);
                Assert.AreEqual("!@#@", result.Data[0].Pattern);
                Assert.AreEqual("all", result.Data[0].AppliedTo);
                Assert.IsNotNull(result.Data[0].Definition);
                Assert.AreEqual("exactly", result.Data[0].Definition["ha-mode"]);
                Assert.AreEqual("automatic", result.Data[0].Definition["ha-sync-mode"]);
                Assert.AreEqual("2", result.Data[0].Definition["ha-params"]);
                Assert.AreEqual(0, result.Data[0].Priority);
            });
        }
        
        [Test]
        public async Task Verify_can_create_policy1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Create("P5", "^amq.", "HareDu", x =>
                {
                    x.SetHighAvailabilityMode(HighAvailabilityModes.All);
                    x.SetExpiry(1000);
                }, PolicyAppliedTo.All, 0);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);

                PolicyRequest request = result.DebugInfo.Request.ToObject<PolicyRequest>(Deserializer.Options);

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual("all", request.Arguments["ha-mode"]);
                Assert.AreEqual("1000", request.Arguments["expires"]);
                Assert.AreEqual(PolicyAppliedTo.All, request.ApplyTo);
            });
         }
        
        [Test]
        public async Task Verify_can_create_policy2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreatePolicy("P5", "^amq.", "HareDu", x =>
                {
                    x.SetHighAvailabilityMode(HighAvailabilityModes.All);
                    x.SetExpiry(1000);
                }, PolicyAppliedTo.All, 0);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);

                PolicyRequest request = result.DebugInfo.Request.ToObject<PolicyRequest>(Deserializer.Options);

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual("all", request.Arguments["ha-mode"]);
                Assert.AreEqual("1000", request.Arguments["expires"]);
                Assert.AreEqual(PolicyAppliedTo.All, request.ApplyTo);
            });
         }

        [Test]
        public async Task Verify_cannot_create_policy1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Create(string.Empty, "^amq.", string.Empty, x =>
                {
                    x.SetHighAvailabilityMode(HighAvailabilityModes.All);
                    x.SetExpiry(1000);
                    x.SetFederationUpstreamSet("all");
                }, PolicyAppliedTo.All, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                PolicyRequest request = result.DebugInfo.Request.ToObject<PolicyRequest>(Deserializer.Options);

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual("all", request.Arguments["ha-mode"]);
                Assert.AreEqual("1000", request.Arguments["expires"]);
                Assert.AreEqual(PolicyAppliedTo.All, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_cannot_create_policy2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreatePolicy(string.Empty, "^amq.", string.Empty, x =>
                {
                    x.SetHighAvailabilityMode(HighAvailabilityModes.All);
                    x.SetExpiry(1000);
                }, PolicyAppliedTo.All, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                PolicyRequest request = result.DebugInfo.Request.ToObject<PolicyRequest>(Deserializer.Options);

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual("all", request.Arguments["ha-mode"]);
                Assert.AreEqual("1000", request.Arguments["expires"]);
                Assert.AreEqual(PolicyAppliedTo.All, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_cannot_create_policy3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Create(string.Empty, "^amq.", string.Empty, x =>
                {
                    x.SetHighAvailabilityMode(HighAvailabilityModes.All);
                    x.SetExpiry(1000);
                }, PolicyAppliedTo.All, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                PolicyRequest request = result.DebugInfo.Request.ToObject<PolicyRequest>(Deserializer.Options);

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual("all", request.Arguments["ha-mode"]);
                Assert.AreEqual("1000", request.Arguments["expires"]);
                Assert.AreEqual(PolicyAppliedTo.All, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_cannot_create_policy4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                    .CreatePolicy(string.Empty, "^amq.", string.Empty, x =>
                    {
                        x.SetHighAvailabilityMode(HighAvailabilityModes.All);
                        x.SetExpiry(1000);
                    }, PolicyAppliedTo.All, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                PolicyRequest request = result.DebugInfo.Request.ToObject<PolicyRequest>(Deserializer.Options);

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual("all", request.Arguments["ha-mode"]);
                Assert.AreEqual("1000", request.Arguments["expires"]);
                Assert.AreEqual(PolicyAppliedTo.All, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_cannot_create_policy5()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Create(string.Empty, "^amq.", string.Empty, x =>
                {
                    x.SetHighAvailabilityMode(HighAvailabilityModes.All);
                    x.SetExpiry(1000);
                    x.SetFederationUpstreamSet("all");
                }, PolicyAppliedTo.All, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                PolicyRequest request = result.DebugInfo.Request.ToObject<PolicyRequest>(Deserializer.Options);

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual("all", request.Arguments["ha-mode"]);
                Assert.AreEqual("1000", request.Arguments["expires"]);
                Assert.AreEqual(PolicyAppliedTo.All, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_cannot_create_policy6()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreatePolicy(string.Empty, "^amq.", string.Empty, x =>
                {
                    x.SetHighAvailabilityMode(HighAvailabilityModes.All);
                    x.SetExpiry(1000);
                    x.SetFederationUpstreamSet("all");
                }, PolicyAppliedTo.All, 0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                PolicyRequest request = result.DebugInfo.Request.ToObject<PolicyRequest>(Deserializer.Options);

                Assert.AreEqual("^amq.", request.Pattern);
                Assert.AreEqual(0, request.Priority);
                Assert.AreEqual("all", request.Arguments["ha-mode"]);
                Assert.AreEqual("1000", request.Arguments["expires"]);
                Assert.AreEqual(PolicyAppliedTo.All, request.ApplyTo);
            });
        }

        [Test]
        public async Task Verify_can_delete_policy1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
                .Delete("P4", "HareDu");
            
            Assert.IsFalse(result.HasFaulted);
        }

        [Test]
        public async Task Verify_can_delete_policy2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeletePolicy("P4", "HareDu");
            
            Assert.IsFalse(result.HasFaulted);
        }

        [Test]
        public async Task Verify_cannot_delete_policy1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<Policy>()
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
                .DeletePolicy(string.Empty, "HareDu");
            
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
                .Object<Policy>()
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
                .DeletePolicy("P4", string.Empty);
            
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
                .Object<Policy>()
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
                .DeletePolicy(string.Empty, string.Empty);
            
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
                .Object<Policy>()
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
                .DeletePolicy(string.Empty, "HareDu");
            
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
                .Object<Policy>()
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
                .DeletePolicy("P4", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }
    }
}
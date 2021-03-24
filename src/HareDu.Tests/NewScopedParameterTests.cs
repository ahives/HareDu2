namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using HareDu.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Serialization;

    [TestFixture]
    public class NewScopedParameterTests :
        HareDuTesting
    {
        [Test]
        public async Task Verify_able_to_get_all_scoped_parameters1()
        {
            var services = GetContainerBuilder("TestData/ScopedParameterInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .GetAll();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(3, result.Data.Count);
                Assert.IsNotNull(result.Data[0]);
                Assert.AreEqual(2, result.Data[0].Value.Count);
                Assert.AreEqual("10", result.Data[0].Value["max-connections"].ToString());
                Assert.AreEqual("value", result.Data[0].Value["max-queues"].ToString());
            });
        }
        
        [Test]
        public async Task Verify_able_to_get_all_scoped_parameters2()
        {
            var services = GetContainerBuilder("TestData/ScopedParameterInfo.json").BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .GetAllScopedParameters();
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsTrue(result.HasData);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(3, result.Data.Count);
                Assert.IsNotNull(result.Data[0]);
                Assert.AreEqual(2, result.Data[0].Value.Count);
                Assert.AreEqual("10", result.Data[0].Value["max-connections"].ToString());
                Assert.AreEqual("value", result.Data[0].Value["max-queues"].ToString());
            });
        }
        
        [Test]
        public async Task Verify_can_create1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create<long>("fake_parameter", 89, "fake_component", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
            
                ScopedParameterRequest<long> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<long>>(Deserializer.Options);

                Assert.AreEqual("fake_component", request.Component);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual(89, request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_can_create2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter<long>("fake_parameter", 89, "fake_component", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
            
                ScopedParameterRequest<long> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<long>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual(89, request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_can_create3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create("fake_parameter", "value", "fake_component", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_can_create4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter("fake_parameter", "value", "fake_component", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create(string.Empty, "value", "fake_component", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.That(request.ParameterName, Is.Empty.Or.Null);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter(string.Empty, "value", "fake_component", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.That(request.ParameterName, Is.Empty.Or.Null);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create(string.Empty, string.Empty, "fake_component", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.That(request.ParameterName, Is.Empty.Or.Null);
                Assert.That(request.ParameterValue, Is.Empty.Or.Null);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter(string.Empty, string.Empty, "fake_component", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.That(request.ParameterName, Is.Empty.Or.Null);
                Assert.That(request.ParameterValue, Is.Empty.Or.Null);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create5()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create("fake_parameter", "value", string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create6()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter("fake_parameter", "value", string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create7()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create("fake_parameter", "value", string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create8()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter("fake_parameter", "value", string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create9()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create("fake_parameter", "value", "fake_component", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.That(request.VirtualHost, Is.Empty.Or.Null);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create10()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter("fake_parameter", "value", "fake_component", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.That(request.VirtualHost, Is.Empty.Or.Null);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create11()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create("fake_parameter", "value", "fake_component", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.That(request.VirtualHost, Is.Empty.Or.Null);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create12()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter("fake_parameter", "value", "fake_component", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.That(request.VirtualHost, Is.Empty.Or.Null);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create13()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create("fake_parameter", "value", "fake_component", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.That(request.VirtualHost, Is.Empty.Or.Null);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create14()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter("fake_parameter", "value", "fake_component", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            
                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.AreEqual("fake_component", request.Component);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.That(request.VirtualHost, Is.Empty.Or.Null);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create15()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create(string.Empty, "value", string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.That(request.ParameterName, Is.Empty.Or.Null);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create16()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter(string.Empty, "value", string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.That(request.ParameterName, Is.Empty.Or.Null);
                Assert.AreEqual("value", request.ParameterValue);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create17()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create(string.Empty, string.Empty, string.Empty,"HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.That(request.ParameterName, Is.Empty.Or.Null);
                Assert.That(request.ParameterValue, Is.Empty.Or.Null);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create18()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter(string.Empty, string.Empty, string.Empty,"HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.That(request.ParameterName, Is.Empty.Or.Null);
                Assert.That(request.ParameterValue, Is.Empty.Or.Null);
                Assert.AreEqual("HareDu", request.VirtualHost);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create19()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create("fake_parameter", "value", string.Empty, string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.That(request.VirtualHost, Is.Empty.Or.Null);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create20()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter("fake_parameter", "value", string.Empty, string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);

                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.That(request.VirtualHost, Is.Empty.Or.Null);
                Assert.AreEqual("fake_parameter", request.ParameterName);
                Assert.AreEqual("value", request.ParameterValue);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create21()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Create(string.Empty, string.Empty, string.Empty, string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(3, result.DebugInfo.Errors.Count);

                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.That(request.VirtualHost, Is.Empty.Or.Null);
                Assert.That(request.ParameterName, Is.Empty.Or.Null);
                Assert.That(request.ParameterValue, Is.Empty.Or.Null);
            });
        }
        
        [Test]
        public async Task Verify_cannot_create22()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .CreateScopeParameter(string.Empty, string.Empty, string.Empty, string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.IsNotNull(result.DebugInfo);
                Assert.AreEqual(3, result.DebugInfo.Errors.Count);

                ScopedParameterRequest<string> request = result.DebugInfo.Request.ToObject<ScopedParameterRequest<string>>(Deserializer.Options);
            
                Assert.That(request.Component, Is.Empty.Or.Null);
                Assert.That(request.VirtualHost, Is.Empty.Or.Null);
                Assert.That(request.ParameterName, Is.Empty.Or.Null);
                Assert.That(request.ParameterValue, Is.Empty.Or.Null);
            });
        }

        [Test]
        public async Task Verify_can_delete1()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete("fake_parameter", "fake_component", "HareDu");
            
            Assert.IsFalse(result.HasFaulted);
        }

        [Test]
        public async Task Verify_can_delete2()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteScopedParameter("fake_parameter", "fake_component", "HareDu");
            
            Assert.IsFalse(result.HasFaulted);
        }

        [Test]
        public async Task Verify_cannot_delete3()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(string.Empty, "fake_component", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete4()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteScopedParameter(string.Empty, "fake_component", "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete5()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete("fake_parameter", string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete6()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteScopedParameter("fake_parameter", string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete7()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete("fake_parameter", "fake_component", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete8()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteScopedParameter("fake_parameter", "fake_component", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(1, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete9()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(string.Empty, string.Empty, "HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete10()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteScopedParameter(string.Empty,string.Empty,"HareDu");
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete11()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(string.Empty, "fake_component", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete12()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteScopedParameter(string.Empty,"fake_component", string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(2, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete13()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .Object<ScopedParameter>()
                .Delete(string.Empty, string.Empty,string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(3, result.DebugInfo.Errors.Count);
            });
        }

        [Test]
        public async Task Verify_cannot_delete14()
        {
            var services = GetContainerBuilder().BuildServiceProvider();
            var result = await services.GetService<IBrokerObjectFactory>()
                .DeleteScopedParameter(string.Empty, string.Empty,string.Empty);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.HasFaulted);
                Assert.AreEqual(3, result.DebugInfo.Errors.Count);
            });
        }
    }
}
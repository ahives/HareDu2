namespace HareDu.Tests
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Fakes;
    using HareDu.Extensions;
    using NUnit.Framework;
    using Serialization;
    using Shouldly;

    [TestFixture]
    public class JsonExtensionsTests
    {
        [Test]
        public async Task Verify_can_convert_http_response_to_object()
        {
            Guid id = Guid.NewGuid();
            FakeObject obj = new FakeObjectImpl(id, DateTimeOffset.UtcNow);

            string jsonString = obj.ToJsonString(Deserializer.Options);
            byte[] requestBytes = Encoding.UTF8.GetBytes(jsonString);
            var content = new ByteArrayContent(requestBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = new HttpResponseMessage {Content = content};

            var data = await response.ToObject<FakeObject>(Deserializer.Options);
            
            data.ShouldNotBeNull();
            data.Id.ShouldBe(id);
        }

        [Test]
        public void Verify_can_convert_string_to_object()
        {
            Guid id = Guid.NewGuid();
            FakeObject obj = new FakeObjectImpl(id, DateTimeOffset.UtcNow);

            var data = obj.ToJsonString(Deserializer.Options).ToObject<FakeObject>(Deserializer.Options);
            
            data.ShouldNotBeNull();
            data.Id.ShouldBe(id);
        }

        [Test]
        public void Verify_can_return_correct_string()
        {
            Guid id = Guid.NewGuid();
            DateTimeOffset timestamp = DateTimeOffset.UtcNow;
            FakeObject obj1 = new FakeObjectImpl(id, timestamp);

            FakeObject obj2 = obj1.ToJsonString(Deserializer.Options).ToObject<FakeObject>(Deserializer.Options);
            
            ShouldBeTestExtensions.ShouldBe(obj2.Timestamp, timestamp);
            ShouldBeTestExtensions.ShouldBe(obj2.Id, id);
        }

        [Test]
        public void Verify_null_object_returns_empty_string()
        {
            FakeObject obj = null;
            
            obj.ToJsonString(Deserializer.Options).ShouldBe(string.Empty);
        }

        
        class FakeObjectImpl :
            FakeObject
        {
            public FakeObjectImpl(Guid id, DateTimeOffset timestamp)
            {
                Id = id;
                Timestamp = timestamp;
            }

            public Guid Id { get; }
            public DateTimeOffset Timestamp { get; }
        }
    }
}
// Copyright 2013-2020 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Core.Tests.Extensions
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Fakes;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class JsonExtensionsTests
    {
        [Test]
        public async Task Verify_can_convert_http_response_to_object()
        {
            Guid id = Guid.NewGuid();
            FakeObject obj = new FakeObjectImpl(id, DateTimeOffset.UtcNow);

            string jsonString = obj.ToJsonString();
            byte[] requestBytes = Encoding.UTF8.GetBytes(jsonString);
            var content = new ByteArrayContent(requestBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = new HttpResponseMessage {Content = content};

            var data = await response.ToObject<FakeObject>();
            
            data.ShouldNotBeNull();
            data.Id.ShouldBe(id);
        }

        [Test]
        public void Verify_can_convert_string_to_object()
        {
            Guid id = Guid.NewGuid();
            FakeObject obj = new FakeObjectImpl(id, DateTimeOffset.UtcNow);

            var data = obj.ToJsonString().ToObject<FakeObject>();
            
            data.ShouldNotBeNull();
            data.Id.ShouldBe(id);
        }

        [Test]
        public void Verify_can_return_correct_string()
        {
            Guid id = Guid.NewGuid();
            DateTimeOffset timestamp = DateTimeOffset.UtcNow;
            FakeObject obj = new FakeObjectImpl(id, timestamp);

            string json = $"{{" +
                          $"\n  \"id\": \"{id}\"," +
                          $"\n  \"timestamp\": \"{timestamp:O}\"" +
                          $"\n}}";
            
            obj.ToJsonString().ShouldBe(json);
        }

        [Test]
        public void Verify_null_object_returns_empty_string()
        {
            FakeObject obj = null;
            
            obj.ToJsonString().ShouldBe(string.Empty);
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
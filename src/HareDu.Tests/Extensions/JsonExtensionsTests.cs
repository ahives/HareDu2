namespace HareDu.Tests.Extensions
{
    using System;
    using HareDu.Extensions;
    using Model;
    using NUnit.Framework;
    using Serialization;

    [TestFixture]
    public class JsonExtensionsTests
    {
        [Test]
        public void Test()
        {
            var requestParams = new ShovelRequestParams(AckMode.OnConfirm,
                5,
                ShovelProtocolType.Amqp091,
                "amqp://user1@localhost",
                "queue1",
                default,
                default,
                0,
                default,
                ShovelProtocolType.Amqp091,
                "amqp://user1@localhost",
                default,
                default,
                "queue2",
                default,
                default);
            var request = new ShovelRequest(requestParams);

            string json = request.ToJsonString(Deserializer.Options);
            
            Console.WriteLine(json);
        }
    }
}
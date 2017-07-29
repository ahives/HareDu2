namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class UserTests :
        HareDuTestBase
    {
        [Test]
        public async Task Test()
        {
            var result = await Client
                .Factory<UserAdmin>()
                .Create("test1", x =>
                {
                    x.Password("test1");
                    x.WithTags(t =>
                    {
                        t.Management();
                        t.Administrator();
                    });
                });
        }

        [Test]
        public async Task Test2()
        {
            Result<IEnumerable<UserInfo>> result = await Client
                .Factory<UserAdmin>()
                .GetAll();

            foreach (var user in result.Data)
            {
                Console.WriteLine("Username: {0}", user.Username);
                Console.WriteLine("PasswordHash: {0}", user.PasswordHash);
                Console.WriteLine("HashingAlgorithm: {0}", user.HashingAlgorithm);
                Console.WriteLine("Tags: {0}", user.Tags);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
            
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine();

        }
    }
}
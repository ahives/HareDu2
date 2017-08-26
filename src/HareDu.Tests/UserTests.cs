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
        public async Task Test2()
        {
            Result<IEnumerable<UserInfo>> result = await Client
                .Factory<User>()
                .GetAllAsync();

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
        
        [Test]
        public async Task Test1()
        {
            Result result = await Client
                .Factory<User>()
                .CreateAsync(x =>
                {
                    x.Username("testuser1");
                    x.Password("testuserpwd1");
//                    x.WithPasswordHash("");
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                });
        }


        [Test]
        public async Task Test()
        {
            Result result = await Client
                .Factory<User>()
                .DeleteAsync(x => x.User(""));
        }
    }
}
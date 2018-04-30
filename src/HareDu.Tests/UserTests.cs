namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class UserTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_users()
        {
            var result = await Client
                .Factory<User>()
                .GetAll();

            foreach (var user in result.Select(x => x.Data))
            {
                Console.WriteLine("Username: {0}", user.Username);
                Console.WriteLine("PasswordHash: {0}", user.PasswordHash);
                Console.WriteLine("HashingAlgorithm: {0}", user.HashingAlgorithm);
                Console.WriteLine("Tags: {0}", user.Tags);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
        
        [Test, Explicit]
        public async Task Test1()
        {
            Result result = await Client
                .Factory<User>()
                .Create(x =>
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


        [Test, Explicit]
        public async Task Test()
        {
            Result result = await Client
                .Factory<User>()
                .Delete(x => x.User(""));
        }
    }
}
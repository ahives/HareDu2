﻿namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class UserTests :
        ResourceTestBase
    {
        [Test, Explicit]
        public async Task Verify_can_get_all_users()
        {
            var result = await Client
                .Resource<User>()
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
        public async Task Verify_can_get_all_users_without_permissions()
        {
            var result = await Client
                .Resource<User>()
                .GetAllWithoutPermissions();

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
            var result = await Client
                .Resource<User>()
                .Create(x =>
                {
                    x.Username("testuser3");
                    x.Password("testuserpwd3");
                    x.WithPasswordHash("gkgfjjhfjh");
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                });
            
            Console.WriteLine(result.ToJsonString());
        }


        [Test, Explicit]
        public async Task Test()
        {
            var result = await Client
                .Resource<User>()
                .Delete(x => x.User(""));
        }
    }
}
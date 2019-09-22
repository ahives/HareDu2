// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Tests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class UserTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<HareDuModule>();

            _container = builder.Build();
        }

        [Test, Explicit]
        public async Task Verify_can_get_all_users()
        {
            var result = await _container.Resolve<IRmqObjectFactory>()
                .Object<User>()
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
            var result = await _container.Resolve<IRmqObjectFactory>()
                .Object<User>()
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
            var result = await _container.Resolve<IRmqObjectFactory>()
                .Object<User>()
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
            var result = await _container.Resolve<IRmqObjectFactory>()
                .Object<User>()
                .Delete(x => x.User(""));
        }
    }
}
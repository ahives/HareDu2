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
namespace HareDu.Tests.BrokerObjects
{
    using Autofac;
    using Core.Extensions;
    using HareDu.Extensions;
    using HareDu.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class UserTests :
        HareDuTesting
    {
        [Test]
        public void Verify_can_get_all_users()
        {
            var container = GetContainerBuilder("TestData/UserInfo1.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .GetAll()
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBe(2);
            result.Data[0].ShouldNotBeNull();
            result.Data[0].Tags.ShouldBe("administrator");
            result.Data[0].Username.ShouldBe("testuser1");
            result.Data[0].PasswordHash.ShouldBe("EeJtW+FJi3yTLMxKFAfXEiNDJB97tHbplPlYM7v4T0pNqMlx");
            result.Data[0].HashingAlgorithm.ShouldBe("rabbit_password_hashing_sha256");
        }
        
        [Test]
        public void Verify_can_get_all_users_without_permissions()
        {
            var container = GetContainerBuilder("TestData/UserInfo2.json").BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .GetAllWithoutPermissions()
                .GetResult();

            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBe(1);
            result.Data[0].ShouldNotBeNull();
            result.Data[0].Tags.ShouldBe("administrator");
            result.Data[0].Username.ShouldBe("testuser2");
            result.Data[0].PasswordHash.ShouldBe("OasGMUAvOCqt8tFnTAZfvxiVsPAaSCMGHFThOvDXjc/exlxB");
            result.Data[0].HashingAlgorithm.ShouldBe("rabbit_password_hashing_sha256");
        }
        
        [Test]
        public void Verify_can_create_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Create(x =>
                {
                    x.Username("testuser3");
                    x.Password("testuserpwd3");
                    x.PasswordHash("gkgfjjhfjh".ComputePasswordHash());
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();

            UserDefinition definition = result.DebugInfo.Request.ToObject<UserDefinition>();
            
            definition.Password.ShouldBe("testuserpwd3");
            definition.Tags.ShouldBe("administrator");
            definition.PasswordHash.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_can_create_2()
        {
            string passwordHash = "gkgfjjhfjh".ComputePasswordHash();
            
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Create(x =>
                {
                    x.Username("testuser3");
                    x.PasswordHash(passwordHash);
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();

            UserDefinition definition = result.DebugInfo.Request.ToObject<UserDefinition>();
            
            definition.Tags.ShouldBe("administrator");
            definition.Password.ShouldBeNullOrEmpty();
            definition.PasswordHash.ShouldBe(passwordHash);
        }
        
        [Test]
        public void Verify_can_create_3()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Create(x =>
                {
                    x.Username("testuser3");
                    x.Password("testuserpwd3");
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();

            UserDefinition definition = result.DebugInfo.Request.ToObject<UserDefinition>();
            
            definition.Tags.ShouldBe("administrator");
            definition.Password.ShouldBe("testuserpwd3");
            definition.PasswordHash.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_can_create_4()
        {
            string passwordHash = "gkgfjjhfjh".ComputePasswordHash();
            
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Create(x =>
                {
                    x.Username("testuser3");
                    x.Password(string.Empty);
                    x.PasswordHash(passwordHash);
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
            result.DebugInfo.ShouldNotBeNull();

            UserDefinition definition = result.DebugInfo.Request.ToObject<UserDefinition>();
            
            definition.Tags.ShouldBe("administrator");
            definition.Password.ShouldBeNullOrEmpty();
            definition.PasswordHash.ShouldBe(passwordHash);
        }
        
        [Test]
        public void Verify_cannot_create_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Create(x =>
                {
                    x.Username(string.Empty);
                    x.Password("testuserpwd3");
                    x.PasswordHash("gkgfjjhfjh".ComputePasswordHash());
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserDefinition definition = result.DebugInfo.Request.ToObject<UserDefinition>();
            
            definition.Password.ShouldBe("testuserpwd3");
            definition.Tags.ShouldBe("administrator");
            definition.PasswordHash.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Create(x =>
                {
                    x.Username("testuser3");
                    x.Password(string.Empty);
                    x.PasswordHash(string.Empty);
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserDefinition definition = result.DebugInfo.Request.ToObject<UserDefinition>();
            
            definition.Tags.ShouldBe("administrator");
            definition.Password.ShouldBeNullOrEmpty();
            definition.PasswordHash.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_3()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Create(x =>
                {
                    x.Username("testuser3");
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.DebugInfo.ShouldNotBeNull();

            UserDefinition definition = result.DebugInfo.Request.ToObject<UserDefinition>();
            
            definition.Tags.ShouldBe("administrator");
            definition.Password.ShouldBeNullOrEmpty();
            definition.PasswordHash.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_4()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Create(x =>
                {
                    x.Username(string.Empty);
                    x.Password(string.Empty);
                    x.PasswordHash(string.Empty);
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();

            UserDefinition definition = result.DebugInfo.Request.ToObject<UserDefinition>();
            
            definition.Tags.ShouldBe("administrator");
            definition.Password.ShouldBeNullOrEmpty();
            definition.PasswordHash.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Verify_cannot_create_5()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Create(x =>
                {
                    x.Username(string.Empty);
                    x.Password(string.Empty);
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                })
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(2);
            result.DebugInfo.ShouldNotBeNull();

            UserDefinition definition = result.DebugInfo.Request.ToObject<UserDefinition>();
            
            definition.Tags.ShouldBe("administrator");
            definition.Password.ShouldBeNullOrEmpty();
            definition.PasswordHash.ShouldBeNullOrEmpty();
        }

        [Test]
        public void Verify_can_delete()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Delete(x => x.User("fake_user"))
                .GetResult();
            
            result.HasFaulted.ShouldBeFalse();
        }

        [Test]
        public void Verify_cannot_delete_1()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Delete(x => x.User(string.Empty))
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }

        [Test]
        public void Verify_cannot_delete_2()
        {
            var container = GetContainerBuilder().BuildServiceProvider();
            var result = container.GetService<IBrokerObjectFactory>()
                .Object<User>()
                .Delete(x => {})
                .GetResult();
            
            result.HasFaulted.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
        }
    }
}
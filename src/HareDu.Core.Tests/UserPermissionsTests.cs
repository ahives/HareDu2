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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class UserPermissionsTests :
        ResourceTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_user_permissions()
        {
            var result = await Client
                .Object<UserPermissions>()
                .GetAll();
            
            foreach (var access in result.Select(x => x.Data))
            {
                Console.WriteLine("VirtualHost: {0}", access.VirtualHost);
                Console.WriteLine("Configure: {0}", access.Configure);
                Console.WriteLine("Read: {0}", access.Read);
                Console.WriteLine("Write: {0}", access.Write);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task TestVerify_can_delete_user_permissions()
        {
            var result = await Client
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.User("");
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });
        }

        [Test, Explicit]
        public async Task Verify_can_create_user_permissions()
        {
            var result = await Client
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.User("");
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern("");
                        c.UsingReadPattern("");
                        c.UsingWritePattern("");
                    });
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });
        }
    }
}
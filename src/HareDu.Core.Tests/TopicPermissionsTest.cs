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
    public class TopicPermissionsTest :
        ResourceTestBase
    {
        [Test]
        public async Task Verify_can_get_all_topic_permissions()
        {
            var result = await Client
                .Object<TopicPermissions>()
                .GetAll();
            
            foreach (var permission in result.Select(x => x.Data))
            {
                Console.WriteLine("VirtualHost: {0}", permission.VirtualHost);
                Console.WriteLine("Exchange: {0}", permission.Exchange);
                Console.WriteLine("Read: {0}", permission.Read);
                Console.WriteLine("Write: {0}", permission.Write);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

            Console.WriteLine(result.ToJsonString());
        }
        
        [Test]
        public void Verify_can_filter_topic_permissions()
        {
            var result = Client
                .Object<TopicPermissions>()
                .GetAll()
                .Where(x => x.VirtualHost == "HareDu");
            
            foreach (var permission in result)
            {
                Console.WriteLine("VirtualHost: {0}", permission.VirtualHost);
                Console.WriteLine("Exchange: {0}", permission.Exchange);
                Console.WriteLine("Read: {0}", permission.Read);
                Console.WriteLine("Write: {0}", permission.Write);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Verify_can_create_user_permissions()
        {
            var result = await Client
                .Object<TopicPermissions>()
                .Create(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu");
                    x.Configure(c =>
                    {
                        c.OnExchange("E4");
                        c.UsingReadPattern(".*");
                        c.UsingWritePattern(".*");
                    });
                });

            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task TestVerify_can_delete_user_permissions()
        {
            var result = await Client
                .Object<TopicPermissions>()
                .Delete(x =>
                {
                    x.User("guest");
                    x.VirtualHost("HareDu7");
                });
            
            Console.WriteLine(result.ToJsonString());
        }
    }
}
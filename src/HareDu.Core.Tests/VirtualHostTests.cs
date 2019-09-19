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
    public class VirtualHostTests :
        ResourceTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_vhosts()
        {
            var result = await Client
                .Object<VirtualHost>()
                .GetAll();

            foreach (var vhost in result.Select(x => x.Data))
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Verify_GetAll_HasResult_works()
        {
            var result = await Client
                .Object<VirtualHost>()
                .GetAll();

            Assert.IsTrue(result.HasData);
        }

        [Test, Explicit]
        public async Task Verify_filtered_GetAll_works()
        {
            var result = Client
                .Object<VirtualHost>()
                .GetAll()
                .Where(x => x.Name == "HareDu");

            foreach (var vhost in result)
            {
                Console.WriteLine("Name: {0}", vhost.Name);
                Console.WriteLine("Tracing: {0}", vhost.Tracing);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task Verify_Create_works()
        {
            Result result = await Client
                .Object<VirtualHost>()
                .Create(x =>
                {
                    x.VirtualHost("HareDu7");
                    x.Configure(c =>
                    {
                        c.WithTracingEnabled();
                    });
                });

            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_Delete_works()
        {
            Result result = await Client
                .Object<VirtualHost>()
                .Delete(x => x.VirtualHost("HareDu7"));

            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_start_vhost()
        {
            Result result = await Client
                .Object<VirtualHost>()
                .Startup("", x => x.On(""));
            
            Console.WriteLine(result.ToJsonString());
        }
    }
}
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
    public class PolicyTests :
        ResourceTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_policies()
        {
            var result = await Client
                .Object<Policy>()
                .GetAll();

            foreach (var policy in result.Select(x => x.Data))
            {
                Console.WriteLine("AppliedTo: {0}", policy.AppliedTo);
                Console.WriteLine("Name: {0}", policy.Name);
                Console.WriteLine("Pattern: {0}", policy.Pattern);
                Console.WriteLine("Priority: {0}", policy.Priority);
                Console.WriteLine("VirtualHost: {0}", policy.VirtualHost);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
        
        [Test, Explicit]
        public async Task Verify_can_create_policy()
        {
            var result = await Client
                .Object<Policy>()
                .Create(x =>
                {
                    x.Policy("P5");
                    x.Configure(p =>
                    {
                        p.UsingPattern("^amq.");
                        p.HasPriority(0);
                        p.HasArguments(d =>
                        {
                            d.SetHighAvailabilityMode(HighAvailabilityModes.All);
                            d.SetExpiry(1000);
                        });
                        p.ApplyTo("all");
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
         }

        [Test, Explicit]
        public async Task Verify_cannot_create_policy()
        {
            var result = await Client
                .Object<Policy>()
                .Create(x =>
                {
                    x.Policy("P4");
                    x.Configure(p =>
                    {
                        p.UsingPattern("^amq.");
                        p.HasPriority(0);
                        p.HasArguments(d =>
                        {
                            d.SetHighAvailabilityMode(HighAvailabilityModes.All);
                            d.SetFederationUpstreamSet("all");
                            d.SetExpiry(1000);
                        });
                        p.ApplyTo("all");
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test, Explicit]
        public async Task Verify_can_delete_policy()
        {
            var result = await Client
                .Object<Policy>()
                .Delete(x =>
                {
                    x.Policy("P4");
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }
    }
}
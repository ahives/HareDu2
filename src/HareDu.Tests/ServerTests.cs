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
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting;

    [TestFixture]
    public class ServerTests :
        HareDuTesting
    {
        [Test]
        public async Task Should_be_able_to_get_all_definitions()
        {
            var container = GetContainerBuilder("TestData/ServerDefinitionInfo.json").Build();
            var result = await container.Resolve<IBrokerObjectFactory>()
                .Object<Server>()
                .GetDefinition();
            
            result.HasFaulted.ShouldBeFalse();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Bindings.Count.ShouldBe(8);
            result.Data.Exchanges.Count.ShouldBe(11);
            result.Data.Queues.Count.ShouldBe(5);
            result.Data.Parameters.Count.ShouldBe(3);
            result.Data.Permissions.Count.ShouldBe(8);
            result.Data.Policies.Count.ShouldBe(2);
            result.Data.Users.Count.ShouldBe(2);
            result.Data.VirtualHosts.Count.ShouldBe(9);
            result.Data.GlobalParameters.Count.ShouldBe(5);
            result.Data.TopicPermissions.Count.ShouldBe(3);
            result.Data.RabbitMqVersion.ShouldBe("3.7.15");
        }
        
        // [Test]
        // public async Task Test()
        // {
        //     var container = GetContainerBuilder("TestData/Test.json").Build();
        //     var result = await container.Resolve<IBrokerObjectFactory>()
        //         .Object<Server>()
        //         .GetDefinition();
        //     
        //     var bindings = result
        //         .Select(x => x.Data)
        //         .Select(x => x.Bindings)
        //         .Where(x => x.DestinationType == "queue")
        //         .Select(x => new
        //         {
        //             x.VirtualHost, x.Source, x.Destination
        //         });
        //
        //     var t = new Dictionary<string, List<Binding>>();
        //     foreach (var binding in bindings)
        //     {
        //         if (t.ContainsKey(binding.VirtualHost))
        //             t[binding.VirtualHost].Add(new BindingImpl(binding.Source, binding.Destination));
        //         else
        //             t[binding.VirtualHost] = new List<Binding>(){new BindingImpl(binding.Source, binding.Destination)};
        //     }
        //
        //     foreach (var binding in t)
        //     {
        //         Console.WriteLine($"Virtual Host: {binding.Key}");
        //         foreach (var b in binding.Value)
        //         {
        //             Console.WriteLine($"Exchange: {b.Source} => Queue: {b.Destination}");
        //         }
        //         Console.WriteLine();
        //         // Console.WriteLine($"Exchange: {binding.Key} => Queue: {binding.Value}");
        //     }
        // }
        //
        // class BindingImpl : Binding
        // {
        //     public BindingImpl(string source, string destination)
        //     {
        //         Source = source;
        //         Destination = destination;
        //     }
        //
        //     public string Source { get; }
        //     public string Destination { get; }
        // }
        //
        // interface Binding
        // {
        //     string Source { get; }
        //     string Destination { get; }
        // }
    }
}
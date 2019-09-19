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
    using Core.Model;
    using NUnit.Framework;

    [TestFixture]
    public class NodeTests :
        ResourceTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_nodes()
        {
            var result = await Client
                .Object<Node>()
                .GetAll();
            
            foreach (var node in result.Select(x => x.Data))
            {
                Console.WriteLine("OperatingSystemPid: {0}", node.OperatingSystemProcessId);
                Console.WriteLine("TotalFileDescriptors: {0}", node.TotalFileDescriptors);
                Console.WriteLine("MemoryUsedDetailsRate: {0}", node.MemoryUsageDetails.Rate);
                Console.WriteLine("FileDescriptorUsedDetailsRate: {0}", node.FileDescriptorUsedDetails.Rate);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }
    }
}
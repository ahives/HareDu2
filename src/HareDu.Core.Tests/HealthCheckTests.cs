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
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class HealthCheckTests :
        ResourceTestBase
    {
        [Test]
        public async Task Verify_IsHealthy_works()
        {
//            Result<Model.ServerHealth> result = await Client
//                .Resource<ServerHealth>()
//                .GetDetails(x =>
//                {
//                    x.VirtualHost("HareDu");
//                });
//
//            Console.WriteLine("Status: {0}", result.Data.Status);
//            Console.WriteLine("****************************************************");
//            Console.WriteLine();
        }
        
        [Test]
        public async Task Verify_GetClusterDetails_works()
        {
//            Result<ClusterInfo> result = await Client
//                .Resource<Cluster>()
//                .GetDetails();
//
//            Console.WriteLine("ClusterName: {0}", result.Data.ClusterName);
////            Console.WriteLine("TotalQueues: {0}", result.Data.ClusterObjects.TotalQueues);
////            Console.WriteLine("TotalConsumers: {0}", result.Data.ClusterObjects.TotalConsumers);
////            Console.WriteLine("TotalExchanges: {0}", result.Data.ClusterObjects.TotalExchanges);
//            Console.WriteLine("RabbitMqVersion: {0}", result.Data.RabbitMqVersion);
//            Console.WriteLine("****************************************************");
//            Console.WriteLine();

//            foreach (var listener in result.Data.Listeners)
//            {
//                Console.WriteLine("Node: {0}", listener.Node);
//                Console.WriteLine("IPAddress: {0}", listener.IPAddress);
//                Console.WriteLine("Port: {0}", listener.Port);
//                Console.WriteLine("****************************************************");
//                Console.WriteLine();
//            }
        }
    }
}
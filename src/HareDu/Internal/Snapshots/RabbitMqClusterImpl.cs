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
namespace HareDu.Internal.Snapshots
{
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public class RabbitMqClusterImpl :
        BaseSnapshot,
        RabbitMqCluster
    {
        public RabbitMqClusterImpl(IResourceFactory factory)
            : base(factory)
        {
        }
        
        public async Task<ClusterSnapshot> GetDetails()
        {
            var clusterResource = await _factory
                .Resource<Cluster>()
                .GetDetails();

            var cluster = clusterResource.Select(x => x.Data);

            var nodeResource = await _factory
                .Resource<Node>()
                .GetAll();

            var nodes = nodeResource.Select(x => x.Data);

            var connectionResource = await _factory
                .Resource<Connection>()
                .GetAll();

            var connections = connectionResource.Select(x => x.Data);

            var channelResource = await _factory
                .Resource<Channel>()
                .GetAll();

            var channels = channelResource.Select(x => x.Data);

            var queueResource = await _factory
                .Resource<Queue>()
                .GetAll();

            var queues = queueResource.Select(x => x.Data);
            
            var summary = new ClusterSnapshotImpl(cluster, nodes, connections, channels, queues);

            return summary;
        }
    }
}
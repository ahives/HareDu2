namespace HareDu.Status
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;

    public interface SomeInterface
    {
        Task<ClusterStatus> GetSummary();
        
//        Task<ClusterState> GetCurrentState();
    }

    public interface ClusterState
    {
    }

    public class SomeInterfaceImpl : SomeInterface
    {
        readonly HareDuFactory _factory;

        public SomeInterfaceImpl(string user, string password)
        {
            _factory = InitializeFactory(user, password);
        }

        HareDuFactory InitializeFactory(string user, string password)
        {
            return HareDuClient.Initialize(x =>
            {
                x.ConnectTo("http://localhost:15672");
                x.UsingCredentials(user, password);
            });
        }
        
        public async Task<ClusterStatus> GetSummary()
        {
            var clusterResource = await _factory
                .Resource<Cluster>()
                .GetDetails();

            var cluster = clusterResource.Select(x => x.Data)[0];

            var nodeResource = await _factory
                .Resource<Node>()
                .GetAll();

            var clusterNodes = nodeResource.Select(x => x.Data);

            var connectionResource = await _factory
                .Resource<Connection>()
                .GetAll();

            var connections = connectionResource.Select(x => x.Data);

            var channelResource = await _factory
                .Resource<Channel>()
                .GetAll();

            var channels = channelResource.Select(x => x.Data);
            
            var summary = new ClusterStatusImpl(cluster, clusterNodes, connections, channels);

            return summary;
        }
    }
}
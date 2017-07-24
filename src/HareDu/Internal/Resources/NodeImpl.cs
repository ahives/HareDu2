namespace HareDu.Internal.Resources
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Logging;
    using Model;

    internal class NodeImpl :
        ResourceBase,
        Node
    {
        public async Task<Result<NodeHealthCheck>> IsHealthy(string node, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);
            
            string url = $"api/healthchecks/node/{node}";

            LogInfo($"Sent request to execute an health check on RabbitMQ server node '{node}'.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<NodeHealthCheck> result = await response.GetResponse<NodeHealthCheck>();

            return result;
        }

        public async Task<Result<IEnumerable<ChannelInfo>>> GetChannels(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/nodes";

            LogInfo("Sent request to return all information on all nodes on current RabbitMQ cluster.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<ChannelInfo>> result = await response.GetResponse<IEnumerable<ChannelInfo>>();

            return result;
        }

        public async Task<Result<IEnumerable<ConnectionInfo>>> GetConnections(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/connections";

            LogInfo($"Sent request to return all connection information on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<ConnectionInfo>> result = await response.GetResponse<IEnumerable<ConnectionInfo>>();

            return result;
        }

        public NodeImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }
    }
}
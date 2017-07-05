namespace HareDu.Internal.Resources
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Logging;
    using Model;

    internal class ServerResourceImpl :
        ResourceBase,
        ServerResource
    {
        public async Task<Result<VirtualHostHealthCheck>> IsVirtualHostHealthy(string vhost, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string sanitizedVHostName = vhost.SanitizeVirtualHostName();
            
            string url = $"api/aliveness-test/{sanitizedVHostName}";

            LogInfo($"Sent request to execute an aliveness test on virtual host '{sanitizedVHostName}' on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<VirtualHostHealthCheck> result = await response.GetResponse<VirtualHostHealthCheck>();

            return result;
        }

        public async Task<Result<NodeHealthCheck>> IsNodeHealthy(string node, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);
            
            string url = $"api/healthchecks/node/{node}";

            LogInfo($"Sent request to execute an health check on RabbitMQ server node '{node}'.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<NodeHealthCheck> result = await response.GetResponse<NodeHealthCheck>();

            return result;
        }

        public async Task<Result<NodeHealthCheck>> IsNodeHealthy(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);
            
            string url = $"api/healthchecks/node";

            LogInfo($"Sent request to execute an health check on current RabbitMQ server node.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<NodeHealthCheck> result = await response.GetResponse<NodeHealthCheck>();

            return result;
        }

        public async Task<Result<Node>> GetNode(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/nodes/{name}";

            LogInfo("Sent request to return all information on all nodes on current RabbitMQ cluster.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<Node> result = await response.GetResponse<Node>();

            return result;
        }

        public async Task<Result<IEnumerable<Node>>> GetAllNodes(CancellationToken cancellationToken)
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/nodes";

            LogInfo("Sent request to return all information on all nodes on current RabbitMQ cluster.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<Node>> result = await response.GetResponse<IEnumerable<Node>>();

            return result;
        }

        public async Task<Result<Cluster>> GetClusterDetails(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/overview";

            LogInfo("Sent request to return information pertaining to the RabbitMQ cluster.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<Cluster> result = await response.GetResponse<Cluster>();

            return result;
        }

        public ServerResourceImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }
    }
}
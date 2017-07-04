namespace HareDu.Internal.Resources
{
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

        public ServerResourceImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }
    }
}
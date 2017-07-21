namespace HareDu.Internal.Resources
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Logging;
    using Exceptions;
    using Model;

    internal class VirtualHostImpl :
        ResourceBase,
        VirtualHost
    {
        public async Task<Result<VirtualHostHealthCheck>> IsHealthy(string vhost, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string sanitizedVHostName = vhost.SanitizeVirtualHostName();
            
            string url = $"api/aliveness-test/{sanitizedVHostName}";

            LogInfo($"Sent request to execute an aliveness test on virtual host '{sanitizedVHostName}' on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<VirtualHostHealthCheck> result = await response.GetResponse<VirtualHostHealthCheck>();

            return result;
        }

        public async Task<Result<VirtualHostDefinition>> GetDefinition(string vhost, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/definitions/{sanitizedVHost}";

            LogInfo($"Sent request to return all information corresponding to virtual host '{sanitizedVHost}' on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<VirtualHostDefinition> result = await response.GetResponse<VirtualHostDefinition>();

            return result;
        }

        public async Task<Result<VirtualHostInfo>> Get(string vhost, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/vhosts/{sanitizedVHost}";

            LogInfo($"Sent request to return all information corresponding to virtual host '{sanitizedVHost}' on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<VirtualHostInfo> result = await response.GetResponse<VirtualHostInfo>();

            return result;
        }

        public async Task<Result<IEnumerable<VirtualHostInfo>>> GetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/vhosts";

            LogInfo("Sent request to return all information on all virtual hosts on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<VirtualHostInfo>> result = await response.GetResponse<IEnumerable<VirtualHostInfo>>();

            return result;
        }

        public async Task<Result> Create(string vhost, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/vhosts/{sanitizedVHost}";

            LogInfo($"Sent request to RabbitMQ server to create virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpPut(url, new StringContent(string.Empty), cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string vhost, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string sanitizedVHost = vhost.SanitizeVirtualHostName();
            
            if (sanitizedVHost == "2%f")
                throw new DeleteVirtualHostException("Cannot delete the default virtual host.");

            string url = $"api/vhosts/{sanitizedVHost}";

            LogInfo($"Sent request to RabbitMQ server to delete virtual host '{vhost}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public VirtualHostImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }
    }
}
namespace HareDu.Internal.Resources
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Logging;
    using Exceptions;
    using Model;

    internal class VirtualHostResourceImpl :
        ResourceBase,
        VirtualHostResource
    {
        public ExchangeResource Exchange { get; }
        public QueueResource Queue { get; }

        public async Task<Result<VirtualHost>> Get(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string sanitizedVHostName = name.SanitizeVirtualHostName();

            string url = $"api/vhosts/{sanitizedVHostName}";

            LogInfo($"Sent request to return all information corresponding to virtual host '{sanitizedVHostName}' on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<VirtualHost> result = await response.GetResponse<VirtualHost>();

            return result;
        }

        public async Task<Result<IEnumerable<VirtualHost>>> GetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/vhosts";

            LogInfo("Sent request to return all information on all virtual hosts on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<VirtualHost>> result = await response.GetResponse<IEnumerable<VirtualHost>>();

            return result;
        }

        public async Task<Result> Create(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string sanitizedVHostName = name.SanitizeVirtualHostName();

            string url = $"api/vhosts/{sanitizedVHostName}";

            LogInfo($"Sent request to RabbitMQ server to create virtual host '{sanitizedVHostName}'.");

            HttpResponseMessage response = await HttpPut(url, new StringContent(string.Empty), cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string sanitizedVHostName = name.SanitizeVirtualHostName();
            
            if (sanitizedVHostName == "2%f")
                throw new DeleteVirtualHostException("Cannot delete the default virtual host.");

            string url = $"api/vhosts/{sanitizedVHostName}";

            LogInfo($"Sent request to RabbitMQ server to delete virtual host '{name}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public VirtualHostResourceImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }
    }
}
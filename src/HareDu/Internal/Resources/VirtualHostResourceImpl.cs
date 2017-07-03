namespace HareDu.Internal.Resources
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Logging;
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

            string url = $"api/vhosts/{name.SanitizeVirtualHostName()}";

            LogInfo($"Sent request to return all information corresponding to virtual host {name} on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<VirtualHost> result = await response.Get<VirtualHost>();

            return result;
        }

        public async Task<Result<IEnumerable<VirtualHost>>> GetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/vhosts";

            LogInfo("Sent request to return all information on all virtual hosts on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<VirtualHost>> result = await response.Get<IEnumerable<VirtualHost>>();

            return result;
        }

        public async Task<Result> Create(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result> Delete(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServerTestResponse> IsAlive(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public VirtualHostResourceImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }
    }
}
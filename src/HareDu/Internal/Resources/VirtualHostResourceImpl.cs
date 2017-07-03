namespace HareDu.Internal.Resources
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Logging;
    using HareDu.Model;
    using Json;
    using Model;

    internal class VirtualHostResourceImpl :
        ResourceBase,
        VirtualHostResource
    {
        public ExchangeResource Exchange { get; }
        public QueueResource Queue { get; }

        public async Task<VirtualHost> Get(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<IEnumerable<VirtualHost>>> GetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/vhosts";

            LogInfo("Sent request to return all information on all virtual hosts on current RabbitMQ server.");

            HttpResponseMessage response = await Get(url, cancellationToken);
            Result<IEnumerable<VirtualHostSummary>> result = await response.Get<IEnumerable<VirtualHostSummary>>();

            return MakeImmutable(result);
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

        Result<IEnumerable<VirtualHost>> MakeImmutable(Result<IEnumerable<VirtualHostSummary>> result)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<VirtualHost> MakeImmutable(IEnumerable<VirtualHostSummary> data)
        {
            foreach (var virtualHostSummary in data)
            {
                yield return new VirtualHostImpl(virtualHostSummary);
            }
        }

        class VirtualHostImpl :
            VirtualHost
        {
            public VirtualHostImpl(VirtualHostSummary virtualHostSummary)
            {
                throw new System.NotImplementedException();
            }

            public string Name { get; }
            public string Tracing { get; }
        }
    }
}
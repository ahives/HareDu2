namespace HareDu.Internal.Resources
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Common.Logging;
    using Model;

    internal class VirtualHostResourceImpl :
        ResourceBase,
        VirtualHostResource
    {
        public ExchangeResource Exchange { get; }
        public QueueResource Queue { get; }
        
        public async Task<IEnumerable<VirtualHost>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServerResponse> Create(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServerResponse> Delete(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServerTestResponse> IsAlive()
        {
            throw new System.NotImplementedException();
        }

        public VirtualHostResourceImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }
    }
}
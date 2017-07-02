namespace HareDu
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;

    public interface VirtualHostResource :
        Resource
    {
        ExchangeResource Exchange { get; }
        QueueResource Queue { get; }

        Task<IEnumerable<VirtualHost>> GetAll();
        Task<ServerResponse> Create(string name);
        Task<ServerResponse> Delete(string name);
        Task<ServerTestResponse> IsAlive();
    }
}
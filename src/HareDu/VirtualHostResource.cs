namespace HareDu
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface VirtualHostResource :
        Resource
    {
        ExchangeResource Exchange { get; }
        QueueResource Queue { get; }

        Task<Result<VirtualHost>> Get(string name, CancellationToken cancellationToken = default(CancellationToken));
        Task<Result<IEnumerable<VirtualHost>>> GetAll(CancellationToken cancellationToken = default(CancellationToken));
        Task<Result> Create(string name, CancellationToken cancellationToken = default(CancellationToken));
        Task<Result> Delete(string name, CancellationToken cancellationToken = default(CancellationToken));
        Task<ServerTestResponse> IsAlive(CancellationToken cancellationToken = default(CancellationToken));
    }
}
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

        Task<Result<Connection>> GetConnections(string vhostName, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<Channel>> GetChannels(string vhostName, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<VirtualHostDefinition>> GetDefinition(string vhostName, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<VirtualHost>> Get(string vhostName, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<IEnumerable<VirtualHost>>> GetAll(CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Create(string vhostName, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Delete(string vhostName, CancellationToken cancellationToken = default(CancellationToken));
    }
}
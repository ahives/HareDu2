namespace HareDu
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface VirtualHostResource :
        Resource
    {
        TResource Factory<TResource>() where TResource : Resource;

        Task<Result<Connection>> GetConnections(string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<Channel>> GetChannels(string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<VirtualHostDefinition>> GetDefinition(string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<VirtualHost>> Get(string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<IEnumerable<VirtualHost>>> GetAll(CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Create(string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Delete(string vhost, CancellationToken cancellationToken = default(CancellationToken));
    }
}
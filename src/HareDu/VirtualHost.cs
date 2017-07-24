namespace HareDu
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface VirtualHost :
        Resource
    {
        Task<Result<VirtualHostHealthCheck>> IsHealthy(string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<VirtualHostDefinition>> GetDefinition(string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<IEnumerable<VirtualHostInfo>>> GetAll(CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Create(string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Delete(string vhost, CancellationToken cancellationToken = default(CancellationToken));
    }
}
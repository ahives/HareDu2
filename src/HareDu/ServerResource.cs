namespace HareDu
{
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface ServerResource :
        Resource
    {
        Task<Result<VirtualHostHealthCheck>> IsVirtualHostHealthy(
            string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<NodeHealthCheck>> IsNodeHealthy(string node, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<NodeHealthCheck>> IsNodeHealthy(CancellationToken cancellationToken = default(CancellationToken));
    }
}
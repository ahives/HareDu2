namespace HareDu
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface Node :
        Resource
    {
        Task<Result<NodeHealthCheck>> IsHealthy(string node, CancellationToken cancellationToken = default(CancellationToken));

        Task<Result<IEnumerable<ChannelInfo>>> GetChannels(CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<IEnumerable<ConnectionInfo>>> GetConnections(CancellationToken cancellationToken = default(CancellationToken));
    }
}
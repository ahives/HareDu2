namespace HareDu
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface Cluster :
        Resource
    {
        Task<Result<ClusterInfo>> GetDetails(CancellationToken cancellationToken = default(CancellationToken));

        Task<Result<IEnumerable<NodeInfo>>> GetAllNodes(CancellationToken cancellationToken = default(CancellationToken));
    }
}
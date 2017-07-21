namespace HareDu.Internal.Resources
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Logging;
    using Model;

    internal class ClusterImpl :
        ResourceBase,
        Cluster
    {
        public ClusterImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }

        public async Task<Result<ClusterInfo>> GetDetails(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/overview";

            LogInfo("Sent request to return information pertaining to the RabbitMQ cluster.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<ClusterInfo> result = await response.GetResponse<ClusterInfo>();

            return result;
        }

        public async Task<Result<IEnumerable<NodeInfo>>> GetAllNodes(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = "api/nodes";

            LogInfo("Sent request to return all information on all nodes on current RabbitMQ cluster.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<NodeInfo>> result = await response.GetResponse<IEnumerable<NodeInfo>>();

            return result;
        }
    }
}
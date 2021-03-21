namespace HareDu.Internal
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using HareDu.Model;

    class ConnectionImpl :
        BaseBrokerObject,
        Connection
    {
        public ConnectionImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<ConnectionInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/connections";
            
            return await GetAll<ConnectionInfo>(url, cancellationToken).ConfigureAwait(false);
        }
    }
}
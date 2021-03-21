namespace HareDu.Internal
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using HareDu.Model;

    class SystemOverviewImpl :
        BaseBrokerObject,
        SystemOverview
    {
        public SystemOverviewImpl(HttpClient client)
            : base(client)
        {
        }

        public Task<Result<SystemOverviewInfo>> Get(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/overview";
            
            return Get<SystemOverviewInfo>(url, cancellationToken);
        }
    }
}
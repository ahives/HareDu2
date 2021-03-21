namespace HareDu.Internal
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using HareDu.Model;
    using Model;

    class ConsumerImpl :
        BaseBrokerObject,
        Consumer
    {
        public ConsumerImpl(HttpClient client)
            : base(client)
        {
        }

        public Task<ResultList<ConsumerInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/consumers";
            
            return GetAll<ConsumerInfo>(url, cancellationToken);
        }
    }
}
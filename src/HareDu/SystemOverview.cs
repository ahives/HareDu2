namespace HareDu
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface SystemOverview :
        BrokerObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the current thread</param>
        /// <returns></returns>
        Task<Result<SystemOverviewInfo>> Get(CancellationToken cancellationToken = default);
    }
}
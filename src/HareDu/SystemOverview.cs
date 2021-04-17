namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    [Obsolete("This interface is deprecated, please use the BrokerSystem interface instead.")]
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
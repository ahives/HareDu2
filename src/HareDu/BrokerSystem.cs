namespace HareDu
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface BrokerSystem :
        BrokerObject
    {
        /// <summary>
        /// Returns various bits of random information that describe the RabbitMQ system.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result<SystemOverviewInfo>> GetSystemOverview(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rebalances all queues in all RabbitMQ virtual hosts.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> RebalanceAllQueues(CancellationToken cancellationToken = default);
    }
}
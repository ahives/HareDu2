namespace HareDu
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface Consumer :
        BrokerObject
    {
        /// <summary>
        /// Returns all consumers on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns></returns>
        Task<ResultList<ConsumerInfo>> GetAll(CancellationToken cancellationToken = default);
    }
}
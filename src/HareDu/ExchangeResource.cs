namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface ExchangeResource
    {
        TResource Factory<TResource>();
        
        Task<Result<Exchange>> Get(string exchangeName, string vhostName, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<IEnumerable<Exchange>>> GetAll(string vhostName, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Create(string exchangeName, string vhostName, Action<ExchangeBehavior> settings, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Delete(string exchangeName, string vhostName, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Delete(string exchangeName, string vhostName, Action<DeleteCondition> condition, CancellationToken cancellationToken = default(CancellationToken));
    }
}
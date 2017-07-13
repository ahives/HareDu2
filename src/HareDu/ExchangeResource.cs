namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface ExchangeResource :
        Resource
    {
        TResource Factory<TResource>() where TResource : Resource;
        
        Task<Result<Exchange>> Get(string exchange, string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result<IEnumerable<Exchange>>> GetAll(string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Create(string exchange, string vhost, Action<ExchangeBehavior> settings, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Delete(string exchange, string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Delete(string exchange, string vhost, Action<ExchangeDeleteCondition> condition, CancellationToken cancellationToken = default(CancellationToken));
    }
}
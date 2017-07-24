namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface Exchange :
        Resource
    {
        Task<Result<IEnumerable<ExchangeInfo>>> GetAll(string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Create(string exchange, string vhost, Action<ExchangeBehavior> behavior,
            CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Delete(string exchange, string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Delete(string exchange, string vhost, Action<ExchangeDeleteCondition> condition,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
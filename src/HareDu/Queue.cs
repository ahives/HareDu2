namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface Queue :
        Resource
    {
        Task<Result<IEnumerable<QueueInfo>>> GetAll(string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Create(string queue, string vhost, Action<QueueBehavior> settings,
            CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Delete(string queue, string vhost, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Result> Delete(string queue, string vhost, Action<QueueDeleteCondition> condition,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
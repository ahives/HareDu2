namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface Binding :
        Resource
    {
        Task<Result<IEnumerable<BindingInfo>>> GetAll(CancellationToken cancellationToken = default(CancellationToken));

        Task<Result> Create(string vhost, string source, string destination, Action<BindingBehavior> behavior,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<Result> Delete(string vhost, string exchange, string queue,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
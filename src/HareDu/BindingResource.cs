namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface BindingResource
    {
        Task<Result<IEnumerable<Binding>>> GetAll(string vhost, CancellationToken cancellationToken = default(CancellationToken));

        Task<Result> Create(string vhost, string source, string destination, Action<BindingBehavior> behavior,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<Result> Delete(string vhost, string exchange, string queue,
            CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface BindingBehavior
    {
    }
}
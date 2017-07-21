namespace HareDu.Internal.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public class BindingResourceImpl :
        BindingResource
    {
        public async Task<Result<IEnumerable<Binding>>> GetAll(string vhost, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<Result> Create(string vhost, string source, string destination, Action<BindingBehavior> behavior,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<Result> Delete(string vhost, string exchange, string queue, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}
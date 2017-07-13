namespace HareDu
{
    using System.Collections.Generic;
    using System.Threading;
    using Model;

    public interface BindingResource
    {
        IEnumerable<Binding> GetAll(string vhost, CancellationToken cancellationToken = default(CancellationToken));
    }
}
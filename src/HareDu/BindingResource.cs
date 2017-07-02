namespace HareDu
{
    using System.Collections.Generic;
    using Model;

    public interface BindingResource
    {
        IEnumerable<Binding> GetAll();
    }
}
namespace HareDu.Internal.Resources
{
    using System.Collections.Generic;

    public interface BindingSetting
    {
        string RoutingKey { get; }
        IDictionary<string, object> Arguments { get; }
        BindingType BindingType { get; }
    }
}
namespace HareDu
{
    using System.Collections.Generic;

    public interface BindingBehavior
    {
        void RoutingKey(string routingKey);
        void WithArguments(IDictionary<string, object> arguments);
        void BindingType(BindingType bindingType);
    }
}
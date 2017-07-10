namespace HareDu
{
    using System;
    using System.Collections.Generic;

    public interface ExchangeBehavior
    {
        void UsingRoutingType(string routingType);

        void UsingRoutingType(Action<ExchangeRoutingType> routingType);

        void IsDurable();

        void IsForInternalUse();

        void WithArguments(IDictionary<string, object> args);

        void AutoDeleteWhenNotInUse();
    }
}
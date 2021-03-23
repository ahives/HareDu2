namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface ScopedParameterInfo
    {
        string VirtualHost { get; }

        string Component { get; }

        string Name { get; }

        IDictionary<string, object> Value { get; }
    }
}
namespace HareDu.Model
{
    using System;

    public interface ShovelInfo
    {
        string Node { get; }

        DateTimeOffset Timestamp { get; }

        string Name { get; }

        string VirtualHost { get; }

        ShovelType Type { get; }

        ShovelState State { get; }
    }
}
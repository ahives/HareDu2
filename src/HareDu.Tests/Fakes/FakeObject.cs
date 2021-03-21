namespace HareDu.Tests.Fakes
{
    using System;

    public interface FakeObject
    {
        Guid Id { get; }
        
        DateTimeOffset Timestamp { get; }
    }
}
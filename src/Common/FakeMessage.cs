namespace Common
{
    using System;

    public interface FakeMessage
    {
        Guid CorrelationId { get; }
        
        DateTime Timestamp { get; }
    }
}